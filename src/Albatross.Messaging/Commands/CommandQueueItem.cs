using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Commands {
	public class CommandQueueItem {
		public ulong OriginalId { get; set; }
		public string OriginalRoute { get; set; }
		
		public string Route => request.Route;
		public ulong Id => request.Id;
		public ulong ServerId => request.ServerId;
		public string CommandType => request.CommandType;
		public bool FireAndForget => request.FireAndForget;
		public bool IsCompleted { get; internal set; }


		private readonly CommandRequest request;
		public CommandQueue Queue { get; init; }
		public IRegisterCommand Registration { get; init; }
		public object Command { get; init; }
		public IMessage? Reply { get; set; }

		public void SetContext(CommandContext context) {
			context.Route = Route;
			context.Id = Id;
			context.OriginalId = OriginalId;
			context.OriginalRoute = OriginalRoute;
			context.Queue = Queue.Name;
		}

		public CommandQueueItem(ulong originalId, string orignalRoute, CommandRequest request, CommandQueue queue, IRegisterCommand registration, object command) {
			this.OriginalId = originalId;
			this.OriginalRoute = orignalRoute;
			this.request = request;
			Queue = queue;
			Registration = registration;
			Command = command;
		}
	}
}
