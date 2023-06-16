using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Albatross.Messaging.DataLogging;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using Albatross.Messaging.Services;
using Albatross.Messaging.ReqRep.Messages;

namespace Albatross.Messaging.ReqRep {
	public class DealerWorker : IWorkerService, IMessagingService, IDisposable {
		private readonly ILogger<DealerWorker> logger;
		private readonly IDataLogWriter dataLogger;
		private readonly DealerSocket socket;
		private readonly NetMQPoller poller;
		private readonly NetMQTimer timer;
		private readonly AtomicCounter<ulong> counter = new AtomicCounter<ulong>();
		private readonly DealerWorkerConfiguration config;
		private readonly IMessageFactory messageFactory;
		private readonly NetMQQueue<IMessage> queue;
		private readonly TimeSpan heartbeatThreshold;

		private bool disposed = false;
		private WorkerState state;
		private DateTime lastHeartbeat = DateTime.MinValue;

		public string Identity => config.Identity;
		public ISet<string> Services => config.Services;

		IDataLogWriter IMessagingService.DataLogger => dataLogger;

		public DealerWorker(DealerWorkerConfiguration config, IMessageFactory messageFactory, ILogger<DealerWorker> logger, IDataLogWriter dataLogger) {
			this.config = config;
			this.messageFactory = messageFactory;
			this.logger = logger;
			this.dataLogger = dataLogger;
			heartbeatThreshold = TimeSpan.FromMilliseconds(config.ActualHeartbeatThreshold);

			socket = new DealerSocket();
			socket.Options.Identity = config.Identity.ToUtf8Bytes();
			socket.ReceiveReady += Socket_ReceiveReady;

			queue = new NetMQQueue<IMessage>();
			queue.ReceiveReady += (_, args) => this.QueueReceiveReady(args, logger);

			timer = new NetMQTimer(TimeSpan.FromMilliseconds(config.ActualHeartbeatInterval));
			timer.Elapsed += Timer_Elapsed;

			poller = new NetMQPoller {
				socket, queue, timer,
			};
		}

		private void Timer_Elapsed(object? sender, NetMQTimerEventArgs e) {
			try {
				if (state != WorkerState.Unavailable) {
					var elapsed = DateTime.Now - lastHeartbeat;
					if (elapsed > heartbeatThreshold) {
						state = WorkerState.Unavailable;
						logger.LogInformation("disconnect: {elapsed:#,#} > {threshold:#,#}", elapsed.TotalMilliseconds, heartbeatThreshold.TotalMilliseconds);
					} else {
						this.Transmit(new WorkerHeartbeat(Identity, counter.NextId()));
					}
				}
			} catch (Exception err) {
				logger.LogError(err, "error process worker service timer event");
			}
		}

		private void Socket_ReceiveReady(object? sender, NetMQSocketEventArgs e) {
			try {
				var frames = e.Socket.ReceiveMultipartMessage();
				var msg = messageFactory.Create(false, frames);
				switch (msg) {
					case Reconnect _:
						this.Transmit(new WorkerConnect(Identity, counter.NextId(), Services));
						break;
					case BrokerConnectOk _:
						lastHeartbeat = DateTime.Now;
						if (state == WorkerState.Unavailable) {
							state = WorkerState.Connected;
						}
						break;
					case ServerAck _:
						lastHeartbeat = DateTime.Now;
						break;
					case BrokerRequest command:
						lastHeartbeat = DateTime.Now;

						break;
					default:
						logger.LogInformation("unhandled dealer worker msg: {msg}", msg);
						break;
				}
			} catch (Exception err) {
				logger.LogError(err, "error processing worker message");
			}
		}

		public void Start() {
			logger.LogInformation("Starging worker and connecting to broker: {broker}", config.EndPoint);
			socket.Connect(config.EndPoint);
			poller.RunAsync();
			var msg = new WorkerConnect(Identity, counter.NextId(), Services);
			queue.Enqueue(msg);
		}

		public void Dispose() {
			if (!disposed) {
				logger.LogInformation("Closing and disposing worker socket for {identity}", Identity);
				poller.Dispose();
				queue.Dispose();
				socket.Dispose();
				disposed = true;
			}
		}

		public void SubmitToQueue(object message) {
			throw new NotImplementedException();
		}
		public void Transmit(IMessage msg) {
			var frames = msg.Create();
			this.dataLogger.Outgoing(msg, frames);
			this.socket.SendMultipartMessage(frames);
		}
	}
}