using Albatross.Messaging.Commands;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Albatross.Messaging.DataLogging;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Text.Json;
using Albatross.Messaging.Services;
using Albatross.Messaging.ReqRep.Messages;

namespace Albatross.Messaging.ReqRep {
	public class DealerClientService : IMessagingService, IClientService {
		private readonly ClientConfiguration config;
		private readonly IMessageFactory messageFactory;
		private readonly ILogger<DealerWorker> logger;
		private readonly IDataLogWriter persistence;
		private readonly MessagingJsonSerializationOption serializerOptions;
		private readonly DealerSocket socket;
		private readonly NetMQPoller poller;
		private readonly NetMQQueue<IMessage> queue;
		private bool disposed = false;
		public string Identity => config.Identity;

		IDataLogWriter IMessagingService.DataLogger => persistence;
		// Dictionary<uint, Command> commands = new Dictionary<uint, Command>();

		public DealerClientService(ClientConfiguration config, IMessageFactory messageFactory, ILogger<DealerWorker> logger,
			IDataLogWriter persistence,
			MessagingJsonSerializationOption serializerOptions) {
			this.config = config;
			this.messageFactory = messageFactory;
			this.logger = logger;
			this.persistence = persistence;
			this.serializerOptions = serializerOptions;
			socket = new DealerSocket();
			socket.ReceiveReady += Socket_ReceiveReady;
			queue = new NetMQQueue<IMessage>();
			queue.ReceiveReady += (_, args) => QueueReceiveReady(this, args, logger);
			poller = new NetMQPoller { socket, queue };
		}

		public void Dispose() {
			if (!disposed) {
				logger.LogInformation("Closing and disposing client socket");
				poller.Dispose();
				queue.Dispose();
				socket.Dispose();
				disposed = true;
			}
		}
		void QueueReceiveReady(IMessagingService svc, NetMQQueueEventArgs<IMessage> args, ILogger logger) {
			try {
				var msg = args.Queue.Dequeue();
				svc.Transmit(msg);
			} catch (Exception ex) {
				logger.LogError(ex, "error sending queue message");
			}
		}


		private void Socket_ReceiveReady(object? sender, NetMQSocketEventArgs e) {
			try {
				var frames = e.Socket.ReceiveMultipartMessage();
				var msg = messageFactory.Create(false, frames);
				switch (msg) {
					case NoAvailableWorker noAvailableWorker:
						AcceptNoAvailableWorker(noAvailableWorker);
						break;
					case WorkerResponse response:
						AcceptResponse(response);
						break;
					default:
						logger.LogInformation("unhandled msg: {msg}", msg);
						break;
				}
			} catch (Exception err) {
				logger.LogError(err, "error processing client message");
			}
		}

		private void AcceptResponse(WorkerResponse response) {
			//if(this.commands.TryGetValue(response.RequestId, out var req)) { 
			//	if(response.Payload.Length > 0) {
			//		var payload = JsonSerializer.Deserialize(response.Payload, req.ResponseType, this.serializerOptions.Default);
			//		if (payload == null) {
			//			req.SetException(new InvalidOperationException("worker returned null response"));
			//		} else {
			//			req.SetResult(payload);
			//		}
			//	}
			//	this.commands.Remove(response.RequestId);
			//}
		}
		//public Task<Respond> Submit<Request, Respond>(Command<Request, Respond> command) {
		//	var bytes = JsonSerializer.SerializeToUtf8Bytes<Request>(command.Request, this.serializerOptions.Default);
		//	var request = new ClientRequest(Identity, command.RequestId, command.Service, bytes);
		//	commands.Add(command.RequestId, command);
		//	this.queue.Enqueue(request);
		//	return command.Task;
		//}
		private void AcceptNoAvailableWorker(NoAvailableWorker msg) {
			//if(commands.TryGetValue(msg.RequestId, out var result)) {
			//	result.SetException(new NoAvailableWorkerException(msg.Service ?? "default"));
			//	commands.Remove(msg.RequestId);
			//}
		}

		public void Start() {
			logger.LogInformation("Starting client and connecting to broker: {endpoint}", config.EndPoint);
			socket.Connect(config.EndPoint);
			poller.RunAsync();
		}

		public void SubmitToQueue(object message) {
			throw new NotImplementedException();
		}
		public void Transmit(IMessage msg) {
			var frames = msg.Create();
			this.persistence.Outgoing(msg, frames);
			this.socket.SendMultipartMessage(frames);
		}
	}
}
