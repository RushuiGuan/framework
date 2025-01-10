using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Core;
using Albatross.Messaging.Services;
using Albatross.Threading;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	public delegate void CommandCallbackDelegate(ulong id, string commandType, byte[] message);
	public delegate void CommandErrorCallbackDelegate(ulong id, string commandType, string errorType, byte[] message);

	public class CommandClientService : IDealerClientService {
		private readonly ILogger<CommandClientService> logger;
		private readonly ConcurrentDictionary<ulong, TaskCallback<ulong>> commandCallbacks = new ConcurrentDictionary<ulong, TaskCallback<ulong>>();

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
				case CommandRequestError requestError:
					AcceptRequestError(dealerClient, requestError);
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
				logger.LogInformation("Running success callback");
				Task.Run(() => {
					try {
						this.OnCommandCompleted(response.Id, response.CommandName, response.Payload);
					} catch (Exception err) {
						logger.LogError(err, "Error running command callback for command {type}({id})", response.CommandName, response.Id);
					}
				});
			}
		}
		private void AcceptError(IMessagingService dealerClient, CommandErrorReply errorMessage) {
			dealerClient.ClientAck(errorMessage.Route, errorMessage.Id);
			if (this.OnCommandError != null) {
				logger.LogInformation("Running error callback");
				Task.Run(() => {
					try {
						this.OnCommandError(errorMessage.Id, errorMessage.CommandName, errorMessage.ClassName, errorMessage.Message);
					} catch (Exception err) {
						logger.LogError(err, "Error running command error callback for {type}({id})", errorMessage.CommandName, errorMessage.Id);
					}
				});
			}
		}
		private void AcceptAckMsg(IMessagingService _, IMessage reply) {
			if (commandCallbacks.Remove(reply.Id, out var callback)) {
				callback.SetResult(reply.Id);
			}
		}
		private void AcceptRequestError(IMessagingService _, CommandRequestError requestError) {
			if (commandCallbacks.Remove(requestError.Id, out var callback)) {
				callback.SetException(new CommandException(requestError.Id, requestError.ClassName, requestError.Message.ToUtf8String()));
			}
		}
		public Task<ulong> Submit(IMessagingService dealerClient, object command, bool fireAndForget) {
			var type = command.GetType();
			var id = dealerClient.Counter.NextId();
			TaskCallback<ulong> callback = new TaskCallback<ulong>();
			if (!commandCallbacks.TryAdd(id, callback)) {
				throw new InvalidOperationException($"Cannot create command callback because of duplicate message id: {id}");
			}
			var bytes = JsonSerializer.SerializeToUtf8Bytes(command, type, MessagingJsonSettings.Value.Default);
			var request = new CommandRequest(string.Empty, id, type.GetCommandName(), fireAndForget ? CommandMode.FireAndForget : CommandMode.Callback, bytes);
			dealerClient.SubmitToQueue(request);
			return callback.Task;
		}
	}
}