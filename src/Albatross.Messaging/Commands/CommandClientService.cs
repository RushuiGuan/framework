﻿using Albatross.Reflection;
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
		private readonly ConcurrentDictionary<ulong, IMessageCallback> commandCallbacks = new ConcurrentDictionary<ulong, IMessageCallback>();

		public CommandClientService(ILogger<CommandClientService> logger) {
			this.logger = logger;
		}

		public bool HasCustomTransmitObject => true;
		public bool CanReceive => true;
		public bool NeedTimer => false;

		public void Init(IMessagingService dealerClient) { }
		public bool ProcessReceivedMsg(IMessagingService dealerClient, IMessage msg) {
			switch (msg) {
				case CommandReply response:
					AcceptResponse(dealerClient, response);
					return true;
				case CommandErrorReply error:
					AcceptError(dealerClient, error);
					return true;
				case CommandQueueStatusReply statusReply:
					AcceptStatusReply(dealerClient, statusReply);
					return true;
				case CommandRequestAck:
				case PingReply:
					AcceptAckMsg(dealerClient, msg);
					return true;
			}
			return false;
		}
		public bool ProcessTransmitQueue(IMessagingService dealerClient, object msg) => false;
		public void ProcessTimerElapsed(DealerClient dealerClient, ulong counter) { }
		private void AcceptStatusReply(IMessagingService _, CommandQueueStatusReply statusReply) {
			if (commandCallbacks.Remove(statusReply.Id, out var callback)) {
				var result = JsonSerializer.Deserialize<CommandQueueInfo[]>(statusReply.Payload, MessagingJsonSettings.Value.Default)
					?? Array.Empty<CommandQueueInfo>();
				callback.SetResult(result);
			} else {
				logger.LogError("command callback not found for message id: {id}", statusReply.Id);
			}
		}
		private void AcceptAckMsg(IMessagingService _, IMessage reply) {
			if (commandCallbacks.Remove(reply.Id, out var callback)) {
				callback.SetResult();
			}
		}
		private void AcceptResponse(IMessagingService dealerClient, CommandReply response) {
			try {
				if (commandCallbacks.Remove(response.Id, out var callback)) {
					if (callback.ResponseType == typeof(void)) {
						callback.SetResult();
					} else {
						var result = JsonSerializer.Deserialize(response.Payload, callback.ResponseType, MessagingJsonSettings.Value.Default);
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
				dealerClient.ClientAck(response.Route, response.Id);
			}
		}
		private void AcceptError(IMessagingService dealerClient, CommandErrorReply errorMessage) {
			try {
				if (commandCallbacks.Remove(errorMessage.Id, out var callback)) {
					callback.SetException(new CommandException(errorMessage.Id, errorMessage.ClassName, errorMessage.Message.ToUtf8String()));
				} else {
					logger.LogError("command callback not found for message id: {id}", errorMessage.Id);
				}
			} finally {
				dealerClient.ClientAck(errorMessage.Route, errorMessage.Id);
			}
		}
		public Task<ResponseType> Submit<CommandType, ResponseType>(DealerClient dealerClient, CommandType command)
			where CommandType : notnull where ResponseType : notnull {

			var id = dealerClient.Counter.NextId();
			logger.LogInformation("the id is {id}, thread {threadid}", id, Environment.CurrentManagedThreadId);
			var callback = new MessageCallback<ResponseType>();
			if (commandCallbacks.TryAdd(id, callback)) {
				var bytes = JsonSerializer.SerializeToUtf8Bytes<CommandType>(command, MessagingJsonSettings.Value.Default);
				var request = new CommandRequest(string.Empty, id, typeof(CommandType).GetClassNameNeat(), false, bytes);
				dealerClient.SubmitToQueue(request);
				return callback.Task;
			} else {
				// this would never happen
				throw new InvalidOperationException($"Cannot create command callback because of duplicate message id: {id}");
			}
		}

		public Task Submit(DealerClient dealerClient, object command, bool fireAndForget) {
			var type = command.GetType();
			var id = dealerClient.Counter.NextId();
			MessageCallback callback = new MessageCallback();
			if (!commandCallbacks.TryAdd(id, callback)) {
				throw new InvalidOperationException($"Cannot create command callback because of duplicate message id: {id}");
			}
			var bytes = JsonSerializer.SerializeToUtf8Bytes(command, type, MessagingJsonSettings.Value.Default);
			var request = new CommandRequest(string.Empty, id, type.GetClassNameNeat(), fireAndForget, bytes);
			dealerClient.SubmitToQueue(request);
			return callback.Task;
		}
		public Task Ping(DealerClient dealerClient) {
			var id = dealerClient.Counter.NextId();
			var callback = new MessageCallback();
			this.commandCallbacks.TryAdd(id, callback);
			dealerClient.SubmitToQueue(new PingRequest(string.Empty, id));
			return callback.Task;
		}
		public Task<CommandQueueInfo[]> QueueStatus(DealerClient dealerClient) {
			var id = dealerClient.Counter.NextId();
			var callback = new MessageCallback<CommandQueueInfo[]>();
			this.commandCallbacks.TryAdd(id, callback);
			dealerClient.SubmitToQueue(new CommandQueueStatus(string.Empty, id));
			return callback.Task;
		}
	}
}