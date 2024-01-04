using System;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Messaging.Services {
	public class TaskCallback<T> {
		public Task<T> Task => taskCompletionSource.Task;
		private TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();

		public TaskCallback() { }
		public TaskCallback(CancellationToken cancellationToken) {
			cancellationToken.Register(() => SetException(new OperationCanceledException(cancellationToken)));
		}
		public TaskCallback(CancellationToken cancellationToken, Action cancellationCallback) {
			cancellationToken.Register(() => cancellationCallback());
		}
		public void SetException(Exception err) {
			taskCompletionSource.TrySetException(err);
		}
		public void SetResult(T obj) {
			taskCompletionSource.TrySetResult(obj);
		}
	}
	public class TaskCallback {
		public Task Task => taskCompletionSource.Task;
		private TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

		public TaskCallback() { }
		public TaskCallback(CancellationToken cancellationToken) {
			cancellationToken.Register(() => SetException(new OperationCanceledException(cancellationToken)));
		}
		public TaskCallback(CancellationToken cancellationToken, Action cancellationCallback) {
			cancellationToken.Register(() => cancellationCallback());
		}
		public void SetException(Exception err) {
			taskCompletionSource.TrySetException(err);
		}
		public void SetResult() {
			taskCompletionSource.TrySetResult(true);
		}
	}
}