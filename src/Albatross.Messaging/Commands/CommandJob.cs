using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Commands {
	public class CommandJob {
		public string Route => request.Route;
		public ulong Id => request.Id;
		public bool FireAndForget => request.FireAndForget;
		public bool IsCompleted { get; internal set; }

		private readonly CommandRequest request;
		public CommandQueue Queue { get; init; }
		public IRegisterCommand Registration { get; init; }
		public object Command { get; init; }
		public IMessage? Reply { get; set; }

		public CommandJob(CommandRequest request, CommandQueue queue, IRegisterCommand registration, object command) {
			this.request = request;
			Queue = queue;
			Registration = registration;
			Command = command;
		}
	}
}
