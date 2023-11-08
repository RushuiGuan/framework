using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Commands {
	public class CommandQueueItem {
		public string Route => request.Route;
		public ulong Id => request.Id;
		public ulong ServerId => request.ServerId;
		public string CommandType => request.CommandType;

		public bool FireAndForget { get; set; }
		public bool IsCompleted { get; internal set; }


		private readonly ICommandRequest request;
		public CommandQueue Queue { get; init; }
		public IRegisterCommand Registration { get; init; }
		public object Command { get; init; }
		public IMessage? Reply { get; set; }

		public CommandQueueItem(CommandRequest request, CommandQueue queue, IRegisterCommand registration, object command) {
			this.request = request;
			Queue = queue;
			Registration = registration;
			Command = command;
			this.FireAndForget = true;
		}

		public CommandQueueItem(CommandRequest2 request, CommandQueue queue, IRegisterCommand registration, object command) {
			this.request = request;
			Queue = queue;
			Registration = registration;
			Command = command;
			this.FireAndForget = false;
		}
	}
}
