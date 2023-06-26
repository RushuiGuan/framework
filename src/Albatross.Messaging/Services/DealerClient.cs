using CoenM.Encoding;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Albatross.Messaging.DataLogging;
using Microsoft.Extensions.Logging;
using NetMQ.Sockets;
using NetMQ;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Albatross.Messaging.Services {
	public class DealerClient : IMessagingService, IDisposable {
		private readonly DealerClientConfiguration config;
		private readonly IEnumerable<IDealerClientService> services;
		private IEnumerable<IDealerClientService> receiveServices;
		private IEnumerable<IDealerClientService> transmitServices;
		private IEnumerable<IDealerClientService> timerServices;
		private readonly IMessageFactory messageFactory;
		private readonly ILogWriter dataWriter;
		private readonly ILogger<DealerClient> logger;
		private readonly DealerSocket socket;
		private readonly NetMQPoller poller;
		private readonly NetMQQueue<object> queue;
		private readonly NetMQTimer timer;
		private bool running = false;
		private bool disposed = false;
		private Client self;
		private AtomicCounter<ulong> counter = new AtomicCounter<ulong>();

		public ILogWriter DataLogger => this.dataWriter;
		public AtomicCounter<ulong> Counter => this.counter;

		public DealerClient(DealerClientConfiguration config, IEnumerable<IDealerClientService> services, IMessageFactory messageFactory, DealerClientLogWriter dataWriter, ILogger<DealerClient> logger) {
			this.config = config;
			this.services = services;
			this.receiveServices = services.Where(args => args.CanReceive).ToArray();
			this.transmitServices = services.Where(args => args.HasCustomTransmitObject).ToArray();
			this.timerServices = services.Where(args => args.NeedTimer).ToArray();
			this.messageFactory = messageFactory;
			this.dataWriter = dataWriter;
			this.logger = logger;
			socket = new DealerSocket();
			socket.ReceiveReady += Socket_ReceiveReady;
			var identity = string.IsNullOrEmpty(config.Identity) ? Z85Extended.Encode(BitConverter.GetBytes(DateTime.UtcNow.Ticks)) : config.Identity;
			socket.Options.Identity = identity.ToUtf8Bytes();
			this.self = new Client(identity);
			if (!config.MaintainConnection) { this.self.Connected(); }
			queue = new NetMQQueue<object>();
			this.queue.ReceiveReady += Queue_ReceiveReady;
			poller = new NetMQPoller { socket, queue };
			timer = new NetMQTimer(config.ActualTimerInterval);
			timer.Elapsed += Timer_Elapsed;
			if (timerServices.Any() || config.MaintainConnection) {
				poller.Add(timer);
			} else {
				timer.Enable = false;
			}
		}

		private void Timer_Elapsed(object? sender, NetMQTimerEventArgs e) {
			if(config.MaintainConnection) {
				if (self.State != ClientState.Dead) {
					var elapsed = DateTime.UtcNow - self.LastHeartbeat;
					if (elapsed > config.HeartbeatThresholdTimeSpan) {
						self.Lost();
						logger.LogInformation("disconnect: {elapsed:#,#} > {threshold:#,#}", elapsed.TotalMilliseconds, config.HeartbeatThresholdTimeSpan.TotalMilliseconds);
					} else {
						this.Transmit(new Heartbeat(string.Empty, counter.NextId()));
					}
				}
			}
			foreach (var service in this.timerServices) {
				try {
					service.ProcessTimerElapsed(this);
				} catch (Exception ex) {
					logger.LogError(ex, "error processing timer elapsed from {type}", service.GetType().FullName);
				}
			}
		}

		private void Queue_ReceiveReady(object? sender, NetMQQueueEventArgs<object> args) {
			try {
				if (running) {
					var item = args.Queue.Dequeue();
					if (item is IMessage msg) {
						this.Transmit(msg);
					} else {
						foreach (var service in this.transmitServices) {
							if (service.ProcessTransmitQueue(this, item)) {
								return;
							}
						}
						logger.LogError("transmit queue item @{item} not processed", item);
					}
				} else {
					logger.LogError("cannot process transmit queue item because the dealer client is being disposed");
				}
			} catch (Exception ex) {
				logger.LogError(ex, "error sending queue message");
			}
		}

		private void Socket_ReceiveReady(object? sender, NetMQSocketEventArgs e) {
			try {
				var frames = e.Socket.ReceiveMultipartMessage();
				var msg = this.messageFactory.Create(frames);
				// the only processing needed for Ack is to persist it in logs
				if (running) {
					if(msg is ConnectOk) {
						self.Connected();
					}else if(msg is Reconnect) {
						this.Transmit(new Connect(string.Empty, counter.NextId()));
						return;
					}else if(msg is HeartbeatAck ack) {
						self.UpdateHeartbeat();
						return;
					}
					foreach (var service in this.receiveServices) {
						if (service.ProcessReceivedMsg(this, msg)) {
							return;
						}
					}
					if (!(msg is ISystemMessage)) {
						logger.LogInformation("unhandled dealer client msg: {msg}", msg);
					}
				} else {
					logger.LogError("incoming message {msg} cannot get processed because the dealer client is being disposed", msg);
				}
			} catch (Exception err) {
				logger.LogError(err, "error parsing dealer client message");
			}
		}

		public void Start() {
			if (!running) {
				running = true;
				logger.LogInformation("starting dealer client and connecting to broker: {endpoint}", config.EndPoint);
				if (config.MaintainConnection) {
					this.SubmitToQueue(new Connect(string.Empty, counter.NextId()));
				}
				foreach (var service in this.services) {
					try {
						service.Init(this);
					} catch (Exception err) {
						logger.LogError(err, "error init dealer client service {name}", service.GetType().FullName);
					}
				}
				this.socket.Connect(config.EndPoint);
				this.poller.RunAsync();
			}
		}

		public void SubmitToQueue(object message) => this.queue.Enqueue(message);

		public void Transmit(IMessage msg) {
			var frames = msg.Create();
			this.dataWriter.WriteLogEntry(new LogEntry(EntryType.Out, msg));
			this.socket.SendMultipartMessage(frames);
		}

		public void Dispose() {
			if (!disposed) {
				running = false;
				logger.LogInformation("closing and disposing dealer client");
				poller.Stop();
				poller.RemoveAndDispose(socket);
				poller.Dispose();
				queue.Dispose();
				disposed = true;
				logger.LogInformation("dealer client disposed");
			}
		}

		public ClientState GetClientState(string identity) {
			if(string.IsNullOrEmpty(identity)) {
				return this.self.State;
			} else {
				throw new NotSupportedException();
			}
		}
	}
}
