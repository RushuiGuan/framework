using System;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Threading {
	public class TaskCallback<T> {
		public object? Context { get; set; }
		public Task<T> Task => taskCompletionSource.Task;
		private TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
		private CancellationTokenRegistration? cancellationTokenRegistration;

		public TaskCallback() { }
		public TaskCallback(CancellationToken cancellationToken) {
			cancellationTokenRegistration = cancellationToken.Register(() => SetException(new OperationCanceledException(cancellationToken)));
		}
		public TaskCallback(CancellationToken cancellationToken, Action<TaskCallback<T>> cancellationCallback) {
			cancellationTokenRegistration = cancellationToken.Register(() => {
				try {
					cancellationCallback(this);
				} catch (Exception err) {
					this.SetException(err);
				}
			});
		}
		public void SetException(Exception err) {
			cancellationTokenRegistration?.Dispose();
			taskCompletionSource.TrySetException(err);
		}
		public void SetResult(T obj) {
			cancellationTokenRegistration?.Dispose();
			taskCompletionSource.TrySetResult(obj);
		}
	}
	public class TaskCallback {
		public object? Context { get; set; }
		public Task Task => taskCompletionSource.Task;
		private TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
		private CancellationTokenRegistration? cancellationTokenRegistration;

		public TaskCallback() { }
		public TaskCallback(CancellationToken cancellationToken) {
			cancellationTokenRegistration = cancellationToken.Register(() => SetException(new OperationCanceledException(cancellationToken)));
		}
		public TaskCallback(CancellationToken cancellationToken, Action<TaskCallback> cancellationCallback) {
			cancellationTokenRegistration = cancellationToken.Register(() => {
				try {
					cancellationCallback(this);
				} catch (Exception err) {
					SetException(err);
				}
			});
		}
		public void SetException(Exception err) {
			cancellationTokenRegistration?.Dispose();
			taskCompletionSource.TrySetException(err);
		}
		public void SetResult() {
			cancellationTokenRegistration?.Dispose();
			taskCompletionSource.TrySetResult(true);
		}
	}
}