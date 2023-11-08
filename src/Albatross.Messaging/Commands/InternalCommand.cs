using Albatross.Messaging.Commands.Messages;
using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	internal class InternalCommand {
		public const string Route = "internal";
		public InternalCommand(CommandRequest request) {
			Request = request;
		}
		public CommandRequest Request { get; }
	}
}
