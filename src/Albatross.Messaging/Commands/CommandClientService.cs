﻿using Albatross.Reflection;
using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Albatross.Messaging.Commands {
	public delegate void CommandCallbackDelegate(ulong id, string commandType, byte[] message);
	public delegate void CommandErrorCallbackDelegate(ulong id, string commandType, string errorType, byte[] message);

	public class CommandClientService : IDealerClientService {
		private readonly ILogger<CommandClientService> logger;
		private readonly ConcurrentDictionary<ulong, IMessageCallback> commandCallbacks = new ConcurrentDictionary<ulong, IMessageCallback>();

		public CommandClientService(ILogger<CommandClientService> logger) {
			this.logger = logger;
		}

		public CommandCallbackDelegate? OnCommandCompleted { get; set; }
		public CommandErrorCallbackDelegate? OnCommandError { get; set; }

		public bool HasCustomTransmitObject => true;
		public bool CanReceive => true;
		public bool NeedTimer => false;

		public void Init(IMessagingService dealerClient) { }
		public bool ProcessReceivedMsg(IMessagingService dealerClient, IMessage msg) {
			switch (msg) {
				/// server will send an ack to confirm the acceptance of the request
				case CommandRequestAck:
					AcceptAckMsg(dealerClient, msg);
					return true;
				case CommandReply reply:
					AcceptResponse(dealerClient, reply);
					return true;
				case CommandErrorReply error:
					AcceptError(dealerClient, error);
					return true;
			}
			return false;
		}
		public bool ProcessQueue(IMessagingService dealerClient, object msg) => false;
		public void ProcessTimerElapsed(IMessagingService dealerClient, ulong counter) { }
		private void AcceptResponse(IMessagingService dealerClient, CommandReply response) {
			dealerClient.ClientAck(response.Route, response.Id);
			if (this.OnCommandCompleted != null) {
				Task.Run(() => {
					try {
						this.OnCommandCompleted(response.Id, response.CommandType, response.Payload);
					} catch (Exception err) {
						logger.LogError(err, "Error running command callback for command {type}({id})", response.CommandType, response.Id);
					}
				});
			}
		}
		private void AcceptError(IMessagingService dealerClient, CommandErrorReply errorMessage) {
			dealerClient.ClientAck(errorMessage.Route, errorMessage.Id);
			if(this.OnCommandError != null) {
				Task.Run(() => {
					try {
						this.OnCommandError(errorMessage.Id, errorMessage.CommandType, errorMessage.ClassName, errorMessage.Message);
					}catch(Exception err) {
						logger.LogError(err, "Error running command error callback for {type}({id})", errorMessage.CommandType, errorMessage.Id);
					}
				});
			}
		}
		private void AcceptAckMsg(IMessagingService _, IMessage reply) {
			if (commandCallbacks.Remove(reply.Id, out var callback)) {
				callback.SetResult();
			}
		}
		public Task Submit(IMessagingService dealerClient, object command, bool fireAndForget) {
			var type = command.GetType();
			var id = dealerClient.Counter.NextId();
			MessageCallback callback = new MessageCallback();
			if (!commandCallbacks.TryAdd(id, callback)) {
				throw new InvalidOperationException($"Cannot create command callback because of duplicate message id: {id}");
			}
			var bytes = JsonSerializer.SerializeToUtf8Bytes(command, type, MessagingJsonSettings.Value.Default);
			var request = new CommandRequest(string.Empty, id, type.GetClassNameNeat(), fireAndForget ? CommandMode.FireAndForget : CommandMode.Callback, bytes);
			dealerClient.SubmitToQueue(request);
			return callback.Task;
		}
	}
}