using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.Threading {
	public static class TaskExtensions {
		public static async Task WithTimeOut(this Task operation, TimeSpan timeout) {
			if (timeout == TimeSpan.Zero || timeout == TimeSpan.MaxValue) {
				await operation;
			} else if (await Task.WhenAny(operation, Task.Delay(timeout)) == operation) {
				await operation;
			} else {
				throw new TimeoutException($"Task timeout after {timeout.TotalMilliseconds:#,#0} milliseconds");
			}
		}

		public static async Task<T> WithTimeOut<T>(this Task<T> operation, TimeSpan timeout) {
			if (timeout == TimeSpan.Zero || timeout == TimeSpan.MaxValue) {
				return await operation;
			} else if (await Task.WhenAny(operation, Task.Delay(timeout)) == operation) {
				return await operation;
			} else {
				throw new TimeoutException($"Task timeout after {timeout.TotalMilliseconds:#,#0} milliseconds");
			}
		}

		public static Task WithTimeOut(this IEnumerable<Task> tasks, TimeSpan timeout) {
			var operations = Task.WhenAll(tasks);
			return operations.WithTimeOut(timeout);
		}
	}
}