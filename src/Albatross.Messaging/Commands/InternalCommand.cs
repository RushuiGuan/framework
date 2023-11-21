using Albatross.Messaging.Commands.Messages;
using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	internal class InternalCommand {
		public InternalCommand(CommandRequest request) {
			Request = request;
		}
		public CommandRequest Request { get; }
		public virtual void SetException(Exception err) { }
		public virtual void SetResult() { }
	}

	internal class InternalCommandWithCallback : InternalCommand {
		private TaskCompletionSource taskCompletionSource = new TaskCompletionSource();
		public Task Task => taskCompletionSource.Task;

		public InternalCommandWithCallback(CommandRequest request) : base(request) { }

		public override void SetException(Exception err) {
			Task.Run(() => taskCompletionSource.SetException(err));
		}
		public override void SetResult() => Task.Run(() => this.taskCompletionSource.SetResult());
	}
}
