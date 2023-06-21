using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Services {
	public interface IMessageCallback {
		Task Task { get; }
		public Type ResponseType { get; }
		void SetResult(object result);
		void SetResult();
		void SetException(Exception error);
		public ulong Id { get; }
	}

	public class MessageCallback<T> : IMessageCallback {
		public ulong Id { get; init; }
		public Type ResponseType => typeof(T);
		Task IMessageCallback.Task => taskCompletionSource.Task;
		public Task<T> Task => taskCompletionSource.Task;
		private TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();

		public MessageCallback(ulong id) {
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
				SetException(new ArgumentException($"message {Id} cannot set result with incorrect type. expected: {typeof(T).Name}, received: {obj?.GetType()?.Name}"));
			}
		}
		public void SetResult() => new NotSupportedException();
	}

	public class MessageCallback : IMessageCallback {
		public ulong Id { get; init; }
		Task IMessageCallback.Task => taskCompletionSource.Task;
		public Task Task => taskCompletionSource.Task;
		public Type ResponseType => typeof(void);
		private TaskCompletionSource taskCompletionSource = new TaskCompletionSource();

		public MessageCallback(ulong id) {
			Id = id;
		}

		public void SetException(Exception err) {
			// make sure this is the last step in this function
			// SetException and SetResult is also a blocking call.  Will causes deadlocks if not kicked off using a different thread
			Task.Run(() => taskCompletionSource.SetException(err));
		}
		public void SetResult(object _) => throw new NotSupportedException();
		public void SetResult() => Task.Run(() => taskCompletionSource.SetResult());
	}
}