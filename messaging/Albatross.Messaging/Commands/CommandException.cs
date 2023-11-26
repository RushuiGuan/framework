using System;
namespace Albatross.Messaging.Commands {
	public class CommandException: Exception {
		public ulong MessageId { get; init;}
		public string ClassName { get;init;}
		public string ErrorMessage { get; init;}

		public CommandException(ulong messageId, string className, string errorMsg): base($"command {messageId} has encountered a {className}.\n{errorMsg}") {
			MessageId = messageId;
			ClassName = className;
			ErrorMessage = errorMsg;
		}
	}
}
