using Albatross.Messaging.Commands.Messages;

namespace Albatross.Messaging.Commands {
	internal class InternalCommand {
		public ulong OriginalId { get; }
		public string OriginalRoute { get; } 


		public const string Route = "internal";
		public InternalCommand(ulong originalId, string originalRoute, CommandRequest request) {
			Request = request;
			OriginalId = originalId;
			OriginalRoute = originalRoute;
		}
		public CommandRequest Request { get; }
	}
}
