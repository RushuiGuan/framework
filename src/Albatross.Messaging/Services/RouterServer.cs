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
	public class RouterServer : IMessagingService, IDisposable {
		private readonly RouterServerConfiguration config;
		private readonly RouterSocket socket;
		private readonly NetMQPoller poller;
		private readonly NetMQQueue<object> queue;
		private readonly NetMQTimer timer;
		private readonly ILogger<RouterServer> logger;
		private readonly IMessageFactory messageFactory;
		private readonly IDataLogWriter dataLogWriter;
		private readonly IDataLogReader dataLogReader;
		private bool running = false;
		private bool disposed = false;
		private IEnumerable<IRouterServerService> receiveServices;
		private IEnumerable<IRouterServerService> transmitServices;
		private IEnumerable<IRouterServerService> timerServices;


		public IDataLogWriter DataLogger => this.dataLogWriter;

		public RouterServer(RouterServerConfiguration config, IEnumerable<IRouterServerService> services, ILogger<RouterServer> logger, IMessageFactory messageFactory, RouterServerLogWriter dataLogWriter, RouterServerLogReader dataLogReader) {
			logger.LogInformation($"Creating {nameof(RouterServer)} instance");
			this.config = config;
			this.receiveServices = services.Where(args => args.CanReceive).ToArray();
			this.transmitServices = services.Where(args=>args.HasCustomTransmitObject).ToArray();
			this.timerServices = services.Where(args=>args.NeedTimer).ToArray();
			this.messageFactory = messageFactory;
			this.dataLogWriter = dataLogWriter;
			this.dataLogReader = dataLogReader;
			this.logger = logger;
			this.socket = new RouterSocket();
			this.socket.ReceiveReady += Socket_ReceiveReady;
			this.socket.Options.ReceiveHighWatermark = config.ReceiveHighWatermark;
			this.socket.Options.SendHighWatermark = config.SendHighWatermark;
			this.queue = new NetMQQueue<object>();
			this.queue.ReceiveReady += Queue_ReceiveReady;
			this.timer = new NetMQTimer(config.ActualTimerInterval);
			this.timer.Elapsed += Timer_Elapsed;
			this.poller = new NetMQPoller { socket, queue, };
			if(timerServices.Any()) {
				this.poller.Add(timer);
			} else {
				timer.Enable = false;
			}
		}

		private void Timer_Elapsed(object? sender, NetMQTimerEventArgs e) {
			foreach(var service in this.timerServices) {
				try {
					service.ProcessTimerElapsed(this);
				}catch(Exception ex) {
					logger.LogError(ex, "error processing timer elapsed from {type}", service.GetType().FullName);
				}
			}
		}

		private void Queue_ReceiveReady(object? sender, NetMQQueueEventArgs<object> e) {
			try {
				if (running) {
					var item = e.Queue.Dequeue();
					switch (item) {
						case IMessage msg:
							this.Transmit(msg);
							break;
						default:
							foreach (var service in this.transmitServices) {
								if (service.ProcessTransmitQueue(this, item)) {
									return;
								}
							}
							if (!(item is ISystemMessage)) {
								logger.LogError("transmit queue item @{item} not processed", item);
							}
							break;
					}
				} else {
					logger.LogError("cannot process transmit queue item because the router server is being disposed");
				}
			} catch (Exception err) {
				logger.LogError(err, "error processing command reply");
			}
		}

		private void Socket_ReceiveReady(object? sender, NetMQSocketEventArgs e) {
			try {
				var frames = e.Socket.ReceiveMultipartMessage();
				var msg = this.messageFactory.Create(true, frames, this.DataLogger);
				if(msg is Ack) { return; }
				if (running) {
					foreach (var service in receiveServices) {
						if (service.ProcessReceivedMsg(this, msg)) {
							return;
						}
					}
					logger.LogInformation("unhandled router server msg: {msg}", msg);
				} else {
					logger.LogError("incoming message {msg} cannot get processed because the router server is being disposed", msg);
				}
			} catch (Exception err) {
				logger.LogError(err, "error parsing router server message");
			}
		}

		public void Start() {
			logger.LogInformation("running log replay");
			int counter = 0;
			this.queue.Enqueue(new StartReplay());
			foreach (var dataLog in this.dataLogReader.ReadLast(TimeSpan.FromMinutes(config.LogCatchUpPeriod))) {
				counter++;
				var record = this.messageFactory.Create(dataLog);
				this.queue.Enqueue(new Replay(record, counter));
			}
			this.queue.Enqueue(new EndReplay());
			if (!running) {
				running = true;
				this.logger.LogInformation("starting router server at {endpoint}", config.EndPoint);
				this.socket.Bind(config.EndPoint);
				this.poller.RunAsync();
			}
		}

		public void SubmitToQueue(object result) => this.queue.Enqueue(result);

		public void Transmit(IMessage msg) {
			var frames = msg.Create();
			this.dataLogWriter.Outgoing(msg, frames);
			this.socket.SendMultipartMessage(frames);
		}

		public void Dispose() {
			if (!disposed) {
				running = false;
				logger.LogInformation("closing and disposing router server");
				poller.Stop();
				poller.RemoveAndDispose(socket);
				poller.Dispose();
				queue.Dispose();
				disposed = true;
				logger.LogInformation("router server disposed");
			}
		}
	}
}