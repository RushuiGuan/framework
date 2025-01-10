using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Commands {
	public class CommandQueueItem {
		public string Route => request.Route;
		public ulong Id => request.Id;
		public string CommandName => request.CommandName;
		public CommandMode Mode => request.Mode;
		public bool IsCompleted { get; internal set; }


		private readonly CommandRequest request;
		public CommandQueue Queue { get; init; }
		public IRegisterCommand Registration { get; init; }
		public object Command { get; init; }
		public IMessage? Reply { get; set; }

		public void SetContext(CommandContext context) {
			context.Route = Route;
			context.Id = Id;
			context.Queue = Queue.Name;
			context.Mode = request.Mode;
		}

		public CommandQueueItem(CommandRequest request, CommandQueue queue, IRegisterCommand registration, object command) {
			this.request = request;
			Queue = queue;
			Registration = registration;
			Command = command;
		}
	}
}