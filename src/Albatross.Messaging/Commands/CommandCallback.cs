using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	public interface ICommandCallback {
		DateTime? Acked { get; set; }
		Task Task { get; }
		public Type ResponseType { get; }
		void SetResult(object result);
		void SetResult();
		void SetException(Exception error);
	}

	public class CommandCallback<T> : ICommandCallback {
		public ulong Id { get; init; }
		public Type ResponseType => typeof(T);
		Task ICommandCallback.Task => taskCompletionSource.Task;
		public Task<T> Task => taskCompletionSource.Task;
		private TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
		public DateTime? Acked { get; set; }

		public CommandCallback(ulong id) {
			Id = id;
		}

		public void SetException(Exception err) {
			// make sure this is the last step in this function
			// SetException and SetResult is also a blocking call.  Will causes deadlocks if not kicked off using a different thread
			System.Threading.Tasks.Task.Run(() => taskCompletionSource.SetException(err));
		}
		public void SetResult(object obj) {
			if (obj is T) {
				// make sure this is the last step in this function
				// SetException and SetResult is also a blocking call.  Will causes deadlocks if not kicked off using a different thread
				System.Threading.Tasks.Task.Run(() => taskCompletionSource.SetResult((T)obj));
			} else {
				this.SetException(new ArgumentException($"command {Id} cannot set result with incorrect type.  expected: {typeof(T).Name}, received: {obj?.GetType()?.Name}"));
			}
		}
		public void SetResult() => new NotSupportedException();
	}

	public class CommandCallback : ICommandCallback {
		public ulong Id { get; init; }
		Task ICommandCallback.Task => taskCompletionSource.Task;
		public Task Task => taskCompletionSource.Task;
		public Type ResponseType => typeof(void);
		public DateTime? Acked { get; set; }
		private TaskCompletionSource taskCompletionSource = new TaskCompletionSource();

		public CommandCallback(ulong id) {
			Id = id;
		}

		public void SetException(Exception err) {
			// make sure this is the last step in this function
			// SetException and SetResult is also a blocking call.  Will causes deadlocks if not kicked off using a different thread
			Task.Run(() => taskCompletionSource.SetException(err));
		}
		public void SetResult(object _) => throw new NotSupportedException();
		public void SetResult() => Task.Run(() => this.taskCompletionSource.SetResult());
	}
}
