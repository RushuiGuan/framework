using Albatross.Messaging.Commands.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	internal class InternalCommand {
		public const string Route = "internal";
		private TaskCompletionSource taskCompletionSource = new TaskCompletionSource();
		public Task Task => taskCompletionSource.Task;

		public InternalCommand(CommandRequest request) {
			Request = request;
		}

		public CommandRequest Request { get; }

		public void SetException(Exception err) {
			Task.Run(() => taskCompletionSource.SetException(err));
		}
		public void SetResult() => Task.Run(() => this.taskCompletionSource.SetResult());
	}
}
