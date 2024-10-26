using Albatross.Threading;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

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

		[Fact]
		public async Task TestDoubleCallback() {
			var mock = new Mock<Action<TaskCallback<int>>>();
			var callback = new Threading.TaskCallback<int>(new CancellationTokenSource(200).Token, mock.Object);
			_ = Task.Delay(100).ContinueWith(x => callback.SetResult(1));
			Assert.Equal(1, await callback.Task);
			await Task.Delay(200);
			mock.Verify(x => x(It.IsAny<TaskCallback<int>>()), Times.Never);
		}
	}
}