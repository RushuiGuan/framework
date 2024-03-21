using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Thrading.Test {
	public class TestTaskCallback {
		[Fact]
		public async Task TestRegularCallback() {
			var callback = new Threading.TaskCallback();
			_ = Task.Delay(100).ContinueWith(x => callback.SetResult());
			await callback.Task;
		}

		[Fact]
		public async Task TestExceptions() {
			var callback = new Threading.TaskCallback();
			_ = Task.Delay(100).ContinueWith(x => callback.SetException(new InvalidOperationException()));
			await Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await callback.Task);
		}

		[Fact]
		public async Task TestCancellation() {
			var callback = new Threading.TaskCallback(new CancellationTokenSource(100).Token);
			await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await callback.Task);
		}

		[Fact]
		public async Task TestCancellationCallback() {
			var callback = new Threading.TaskCallback(new CancellationTokenSource(100).Token, x => x.SetException(new ArgumentException()));
			await Assert.ThrowsAnyAsync<ArgumentException>(async () => await callback.Task);
		}

		[Fact]
		public async Task TestCancellationCallback2() {
			var callback = new Threading.TaskCallback(new CancellationTokenSource(100).Token, x => throw new ArgumentException());
			await Assert.ThrowsAnyAsync<ArgumentException>(async () => await callback.Task);
		}
	}
}
