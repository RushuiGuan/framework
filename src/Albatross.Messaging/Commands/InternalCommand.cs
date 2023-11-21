using Albatross.Messaging.Commands.Messages;

namespace Albatross.Messaging.Commands {
	internal class InternalCommand {
		public InternalCommand(CommandRequest request) {
			Request = request;
		}
		public CommandRequest Request { get; }
	}
}
