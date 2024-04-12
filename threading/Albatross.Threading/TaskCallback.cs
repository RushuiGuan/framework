using System;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Threading {
	public class TaskCallback<T> {
		public object? Context { get; set; }
		public Task<T> Task => taskCompletionSource.Task;
		private TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();

		public TaskCallback() { }
		public TaskCallback(CancellationToken cancellationToken) {
			cancellationToken.Register(() => SetException(new OperationCanceledException(cancellationToken)));
		}
		public TaskCallback(CancellationToken cancellationToken, Action<TaskCallback<T>> cancellationCallback) {
			cancellationToken.Register(() => {
				try {
					cancellationCallback(this);
				} catch (Exception err) {
					this.SetException(err);
				}
			});
		}
		public void SetException(Exception err) {
			taskCompletionSource.TrySetException(err);
		}
		public void SetResult(T obj) {
			taskCompletionSource.TrySetResult(obj);
		}
	}
	public class TaskCallback {
		public object? Context { get; set; }
		public Task Task => taskCompletionSource.Task;
		private TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

		public TaskCallback() { }
		public TaskCallback(CancellationToken cancellationToken) {
			cancellationToken.Register(() => SetException(new OperationCanceledException(cancellationToken)));
		}
		public TaskCallback(CancellationToken cancellationToken, Action<TaskCallback> cancellationCallback) {
			cancellationToken.Register(() => {
				try {
					cancellationCallback(this);
				} catch (Exception err) {
					SetException(err);
				}
			});
		}
		public void SetException(Exception err) {
			taskCompletionSource.TrySetException(err);
		}
		public void SetResult() {
			taskCompletionSource.TrySetResult(true);
		}
	}
}