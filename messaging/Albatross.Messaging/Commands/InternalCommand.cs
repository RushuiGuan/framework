using Albatross.Messaging.Commands.Messages;
using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	/// <summary>
	/// this class is used by command replay only
	/// </summary>
	internal class InternalCommand {
		public InternalCommand(CommandRequest request) {
			Request = request;
		}
		public CommandRequest Request { get; }
		public virtual void SetException(Exception err) { }
		public virtual void SetResult(ulong id) { }
		public bool Priority { get; set; }
	}

	/// <summary>
	/// this class is used by <see cref="InternalCommandClient"/>
	/// </summary>
	internal class InternalCommandWithCallback : InternalCommand {
		private TaskCompletionSource<ulong> taskCompletionSource = new TaskCompletionSource<ulong>();
		public Task<ulong> Task => taskCompletionSource.Task;

		public InternalCommandWithCallback(CommandRequest request) : base(request) { }

		public override void SetException(Exception err) => taskCompletionSource.TrySetException(err);
		public override void SetResult(ulong id) => this.taskCompletionSource.TrySetResult(id);
	}
}