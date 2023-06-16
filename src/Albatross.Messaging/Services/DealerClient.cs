using CoenM.Encoding;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Albatross.Messaging.DataLogging;
using Microsoft.Extensions.Logging;
using NetMQ.Sockets;
using NetMQ;
using System.Collections.Generic;
using System;

namespace Albatross.Messaging.Services {
	public class DealerClient : IMessagingService, IDisposable {
		private readonly DealerClientConfiguration config;
		private readonly IEnumerable<IDealerClientService> services;
		private readonly IMessageFactory messageFactory;
		private readonly IDataLogWriter dataWriter;
		private readonly ILogger<DealerClient> logger;
		private readonly DealerSocket socket;
		private readonly NetMQPoller poller;
		private readonly NetMQQueue<object> queue;
		private bool running = false;
		private bool disposed = false;
		public string Identity { get; init; }

		IDataLogWriter IMessagingService.DataLogger => this.dataWriter;
		NetMQSocket IMessagingService.Socket => this.socket;

		public DealerClient(DealerClientConfiguration config, IEnumerable<IDealerClientService> services, IMessageFactory messageFactory, IDataLogWriter dataWriter, ILogger<DealerClient> logger) {
			this.config = config;
			this.services = services;
			this.messageFactory = messageFactory;
			this.dataWriter = dataWriter;
			this.logger = logger;
			socket = new DealerSocket();
			socket.ReceiveReady += Socket_ReceiveReady;
			if (string.IsNullOrEmpty(config.Identity)) {
				this.Identity = Z85Extended.Encode(BitConverter.GetBytes(DateTime.UtcNow.Ticks));
			} else {
				this.Identity = config.Identity;
			}
			socket.Options.Identity = this.Identity.ToUtf8Bytes();
			queue = new NetMQQueue<object>();
			this.queue.ReceiveReady += Queue_ReceiveReady;
			poller = new NetMQPoller { socket, queue };
		}

		private void Queue_ReceiveReady(object? sender, NetMQQueueEventArgs<object> args) {
			try {
				if (running) {
					var item = args.Queue.Dequeue();
					if (item is IMessage msg) {
						this.Transmit(msg);
					} else {
						foreach (var service in this.services) {
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
				var msg = this.messageFactory.Create(false, frames);
				// the only processing needed for Ack is to persist it in logs
				if (msg is Ack) { return; }
				if (running) {
					foreach (var service in this.services) {
						if (service.ProcessReceivedMsg(this, msg)) {
							return;
						}
					}
					logger.LogInformation("unhandled dealer client msg: {msg}", msg);
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
				this.socket.Connect(config.EndPoint);
				this.poller.RunAsync();
			}
		}

		public void SubmitToQueue(object message) => this.queue.Enqueue(message);

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
	}
}
