using Albatross.Reflection;
using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	public delegate void CommandCallbackDelegate(ulong id, string commandType, byte[] message);
	public delegate void CommandErrorCallbackDelegate(ulong id, string commandType, string errorType, byte[] message);

	public class CommandClientService : IDealerClientService {
		private readonly ILogger<CommandClientService> logger;
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
					return true;
				/// legacy command support.  
				/// Send back a client ack to maintain backward compatibility.  
				/// "Fire and Wait" mode is no longer supported
				case CommandReply:
				case CommandErrorReply:
					var commandMsg = (Message)msg;
					dealerClient.ClientAck(commandMsg.Route, commandMsg.Id);
					return true;
				case CommandReply2 reply:
					AcceptResponse(dealerClient, reply);
					return true;
				case CommandErrorReply2 error:
					AcceptError(dealerClient, error);
					return true;
			}
			return false;
		}
		public bool ProcessQueue(IMessagingService dealerClient, object msg) => false;
		public void ProcessTimerElapsed(DealerClient dealerClient, ulong counter) { }
		private void AcceptResponse(IMessagingService dealerClient, CommandReply2 response) {
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
		private void AcceptError(IMessagingService dealerClient, CommandErrorReply2 errorMessage) {
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
		
		public ulong Submit(DealerClient dealerClient, object command, bool fireAndForget) {
			var type = command.GetType();
			var id = dealerClient.Counter.NextId();
			var bytes = JsonSerializer.SerializeToUtf8Bytes(command, type, MessagingJsonSettings.Value.Default);
			var request = new CommandRequest(string.Empty, id, type.GetClassNameNeat(), fireAndForget, bytes);
			dealerClient.SubmitToQueue(request);
			return id;
		}
	}
}