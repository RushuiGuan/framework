using Albatross.Reflection;
using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	public class CommandClientService : IDealerClientService {
		private readonly ILogger<CommandClientService> logger;
		private readonly MessagingJsonSerializationOption serializerOptions;
		private readonly Dictionary<Type, IRegisterCommand> registrations = new Dictionary<Type, IRegisterCommand>();
		private readonly ConcurrentDictionary<ulong, ICommandCallback> commandCallbacks = new ConcurrentDictionary<ulong, ICommandCallback>();
		private AtomicCounter counter = new AtomicCounter();


		public CommandClientService(IEnumerable<IRegisterCommand> registrations, ILogger<CommandClientService> logger, MessagingJsonSerializationOption serializerOptions) {
			this.logger = logger;
			this.serializerOptions = serializerOptions;
			foreach (var item in registrations) {
				this.registrations[item.CommandType] = item;
			}
		}

		public bool CanTransmit => true;
		public bool CanReceive => true;

		public bool ProcessReceivedMsg(DealerClient dealerClient, IMessage msg) {
			switch (msg) {
				case CommandReply response:
					AcceptResponse(dealerClient, response);
					break;
				case CommandErrorReply error:
					AcceptError(dealerClient, error);
					break;
				case CommandQueueStatusReply statusReply:
					AcceptStatusReply(dealerClient, statusReply);
					break;
				case CommandRequestAck:
				case PingReply:
					AcceptAckMsg(dealerClient, msg);
					break;
				default:
					return false;
			}
			return true;
		}
		public bool ProcessTransmitQueue(DealerClient dealerClient, object msg) => false;

		private void AcceptStatusReply(DealerClient _, CommandQueueStatusReply statusReply) {
			if (commandCallbacks.Remove(statusReply.Id, out var callback)) {
				var result = JsonSerializer.Deserialize<CommandQueueInfo[]>(statusReply.Payload, this.serializerOptions.Default)
					?? Array.Empty<CommandQueueInfo>();
				callback.SetResult(result);
			} else {
				logger.LogError("command callback not found for message id: {id}", statusReply.Id);
			}
		}

		private void AcceptAckMsg(DealerClient _, IMessage reply) {
			if (commandCallbacks.Remove(reply.Id, out var callback)) {
				callback.SetResult();
			}
		}

		private void AcceptResponse(DealerClient dealerClient, CommandReply response) {
			try {
				if (commandCallbacks.Remove(response.Id, out var callback)) {
					if (callback.ResponseType == typeof(void)) {
						callback.SetResult();
					} else {
						var result = JsonSerializer.Deserialize(response.Payload, callback.ResponseType, this.serializerOptions.Default);
						if (result != null) {
							callback.SetResult(result);
						} else {
							callback.SetException(new InvalidDataException($"command {response.Id} didn't return any response"));
						}
					}
				} else {
					logger.LogError("command callback not found for message id: {id}", response.Id);
				}
			} finally {
				dealerClient.Ack(response.Route, response.Id);
			}
		}
	
		private void AcceptError(DealerClient dealerClient, CommandErrorReply errorMessage) {
			try {
				if (commandCallbacks.Remove(errorMessage.Id, out var callback)) {
					callback.SetException(new CommandException(errorMessage.Id, errorMessage.ClassName, errorMessage.Message));
				} else {
					logger.LogError("command callback not found for message id: {id}", errorMessage.Id);
				}
			} finally {
				dealerClient.Ack(errorMessage.Route, errorMessage.Id);
			}
		}

		public Task<ResponseType> Submit<CommandType, ResponseType>(DealerClient dealerClient, CommandType command)
			where CommandType : Command<ResponseType>
			where ResponseType : notnull {

			var id = counter.NextId();
			logger.LogInformation("the id is {id}, thread {threadid}", id, Environment.CurrentManagedThreadId);
			var callback = new CommandCallback<ResponseType>(id);
			if (commandCallbacks.TryAdd(id, callback)) {
				var bytes = JsonSerializer.SerializeToUtf8Bytes<CommandType>(command, this.serializerOptions.Default);
				var request = new CommandRequest(string.Empty, id, typeof(CommandType).GetClassNameNeat(), false, bytes);
				dealerClient.SubmitToQueue(request);
				return callback.Task;
			} else {
				// this would never happen
				throw new InvalidOperationException($"Cannot create command callback because of duplicate message id: {id}");
			}
		}

		public Task Submit<CommandType>(DealerClient dealerClient, CommandType command, bool fireAndForget = true) where CommandType : Command {
			var id = counter.NextId();
			CommandCallback callback = new CommandCallback(id);
			if (!commandCallbacks.TryAdd(id, callback)) {
				throw new InvalidOperationException($"Cannot create command callback because of duplicate message id: {id}");
			}
			var bytes = JsonSerializer.SerializeToUtf8Bytes(command, this.serializerOptions.Default);
			var request = new CommandRequest(string.Empty, id, typeof(CommandType).GetClassNameNeat(), fireAndForget, bytes);
			dealerClient.SubmitToQueue(request);
			return callback.Task;
		}
		public Task Ping(DealerClient dealerClient) {
			var id = counter.NextId();
			var callback = new CommandCallback(id);
			this.commandCallbacks.TryAdd(id, callback);
			dealerClient.SubmitToQueue(new PingRequest(string.Empty, id));
			return callback.Task;
		}

		public Task<CommandQueueInfo[]> QueueStatus(DealerClient dealerClient) {
			var id = counter.NextId();
			var callback = new CommandCallback<CommandQueueInfo[]>(id);
			this.commandCallbacks.TryAdd(id, callback);
			dealerClient.SubmitToQueue(new CommandQueueStatus(string.Empty, id));
			return callback.Task;
		}
	}
}
