using System;
namespace Albatross.Messaging.Commands {
	public class CommandException : Exception {
		public ulong MessageId { get; init; }
		public string CommandName { get; init; }
		public string ErrorMessage { get; init; }

		public CommandException(ulong messageId, string commandName, string errorMsg) : base($"command  {commandName} with the id of {messageId} has encountered an error.\n{errorMsg}") {
			MessageId = messageId;
			CommandName = commandName;
			ErrorMessage = errorMsg;
		}
	}
}