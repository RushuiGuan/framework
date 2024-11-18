using Albatross.Math;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Messaging.Services {
	public class DealerClient : IMessagingService, IDisposable {
		private readonly DealerClientConfiguration config;
		private readonly IEnumerable<IDealerClientService> services;
		private IEnumerable<IDealerClientService> receiveServices;
		private IEnumerable<IDealerClientService> transmitServices;
		private IEnumerable<IDealerClientService> timerServices;
		private readonly IMessageFactory messageFactory;
		private readonly DiskStorageEventWriter eventWriter;
		private readonly ILogger<DealerClient> logger;
		private readonly DealerSocket socket;
		private readonly NetMQPoller poller;
		private readonly NetMQQueue<object> queue;
		private readonly NetMQTimer timer;
		private bool running = false;
		private bool disposed = false;
		private Client self;
		private IAtomicCounter counter;
		private ulong timerCounter;

		public IEventWriter EventWriter => this.eventWriter;
		public IAtomicCounter Counter => this.counter;

		public DealerClient(DealerClientConfiguration config, IEnumerable<IDealerClientService> services, IMessageFactory messageFactory, ILoggerFactory loggerFactory) {
			this.config = config;
			this.services = services;
			this.receiveServices = services.Where(args => args.CanReceive).ToArray();
			this.transmitServices = services.Where(args => args.HasCustomTransmitObject).ToArray();
			this.timerServices = services.Where(args => args.NeedTimer).ToArray();
			this.messageFactory = messageFactory;
			this.counter = new DurableAtomicCounter(config.DiskStorage.WorkingDirectory);
			this.eventWriter = new DiskStorageEventWriter(config.DiskStorage.FileName, config.DiskStorage, loggerFactory);
			this.logger = loggerFactory.CreateLogger<DealerClient>();
			socket = new DealerSocket();
			if (config.UseCurveEncryption) {
				if (string.IsNullOrEmpty(config.ServerPublicKey)) {
					throw new ArgumentException("curve encryption is enabled but the public key is not provided");
				}
				socket.Options.CurveCertificate = new NetMQCertificate();
				socket.Options.CurveServerKey = NetMQCertificate.FromPublicKey(config.ServerPublicKey).PublicKey;
			}
			socket.ReceiveReady += Socket_ReceiveReady;
			var identity = string.IsNullOrEmpty(config.Identity) ? DateTime.UtcNow.Ticks.ToMaxBase() : $"{config.Identity}_{System.Net.Dns.GetHostName()}";
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
		public string Identity => this.self.Identity;

		private void Timer_Elapsed(object? sender, NetMQTimerEventArgs e) {
			timerCounter++;
			if (config.MaintainConnection) {
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
					service.ProcessTimerElapsed(this, timerCounter);
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
							if (service.ProcessQueue(this, item)) {
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
				this.eventWriter.WriteEvent(new EventEntry(EntryType.In, msg));
				// any msg from router server will update the heartbeat
				self.UpdateHeartbeat();
				// the only processing needed for Ack is to persist it in logs
				if (running) {
					if (msg is ConnectOk) {
						self.Connected();
					} else if (msg is Reconnect) {
						this.Transmit(new Connect(string.Empty, counter.NextId()));
						return;
					} else if (msg is HeartbeatAck ack) {
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
				logger.LogInformation("starting dealer client {identity} and connecting to broker: {endpoint}", this.Identity, config.EndPoint);
				this.socket.Connect(config.EndPoint);
				// wait a second here.  if we start transmitting messages right away, it will get lost
				Task.Delay(1000).Wait();

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
				this.poller.RunAsync();
			}
		}

		public void SubmitToQueue(object message) => this.queue.Enqueue(message);

		public void Transmit(IMessage msg) {
			var frames = msg.Create();
			this.eventWriter.WriteEvent(new EventEntry(EntryType.Out, msg));
			this.socket.SendMultipartMessage(frames);
		}

		public void Dispose() {
			if (!disposed) {
				running = false;
				logger.LogInformation("closing and disposing dealer client {identity}", this.Identity);
				poller.Stop();
				poller.RemoveAndDispose(socket);
				poller.Dispose();
				queue.Dispose();
				this.eventWriter.Dispose();
				disposed = true;
				logger.LogInformation("dealer client {identity} disposed", this.Identity);
			}
		}

		public ClientState GetClientState(string identity) {
			if (string.IsNullOrEmpty(identity)) {
				return this.self.State;
			} else {
				throw new NotSupportedException();
			}
		}
	}
}