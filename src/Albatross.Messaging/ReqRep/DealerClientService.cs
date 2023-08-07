using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Albatross.Messaging.DataLogging;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using System;
using Albatross.Messaging.Services;
using Albatross.Messaging.ReqRep.Messages;

namespace Albatross.Messaging.ReqRep {
	public class DealerClientService : IMessagingService, IClientService {
		private readonly ClientConfiguration config;
		private readonly IMessageFactory messageFactory;
		private readonly ILogger<DealerWorkerService> logger;
		private readonly ILogWriter persistence;
		private readonly DealerSocket socket;
		private readonly NetMQPoller poller;
		private readonly NetMQQueue<IMessage> queue;
		private bool disposed = false;
		public string Identity => config.Identity;

		ILogWriter IMessagingService.DataLogger => persistence;

		public IAtomicCounter<ulong> Counter => throw new NotImplementedException();

		public DealerClientService(ClientConfiguration config, IMessageFactory messageFactory, ILogger<DealerWorkerService> logger,
			ILogWriter logWriter) {
			this.config = config;
			this.messageFactory = messageFactory;
			this.logger = logger;
			this.persistence = logWriter;
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
				var msg = messageFactory.Create(frames);
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
			this.persistence.WriteLogEntry(new LogEntry(EntryType.Out, msg));
			this.socket.SendMultipartMessage(msg.Create());
		}

		public ClientState GetClientState(string identity) {
			throw new NotImplementedException();
		}
	}
}
