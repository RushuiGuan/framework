using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.CommandLine {
	public static class SpectreExtensions {
		public static Queue<ProgressTask> Enqueue(this Queue<ProgressTask> queue, ProgressContext context, string description) {
			queue.Enqueue(context.AddTask(description, false, 1));
			return queue;
		}
		public static async Task<T> ExecuteAsync<T>(this ProgressTask progress, Func<ProgressTask, double, Task<T>> func, Func<string, T, string>? updateDescription = null) {
			progress.StartTask();
			try {
				var counter = 0.0;
				var result = await func(progress, counter);
				if (updateDescription != null) {
					progress.Description = updateDescription(progress.Description, result);
				}
				return result;
			} finally {
				progress.Value = progress.MaxValue;
				progress.StopTask();
			}
		}
		public static T Execute<T>(this ProgressTask progress, Func<ProgressTask, double, T> func, Func<string, T, string>? updateDescription = null) {
			progress.StartTask();
			try {
				var counter = 0.0;
				var result = func(progress, counter);
				if (updateDescription != null) {
					progress.Description = updateDescription(progress.Description, result);
				}
				return result;
			} finally {
				progress.Value = progress.MaxValue;
				progress.StopTask();
			}
		}
		public static async Task ExecuteAsync(this ProgressTask progress, Func<ProgressTask, double, Task> func) {
			progress.StartTask();
			try {
				var counter = 0.0;
				await func(progress, counter);
			} finally {
				progress.Value = progress.MaxValue;
				progress.StopTask();
			}
		}
		public static void Execute(this ProgressTask progress, Action<ProgressTask, double> func) {
			progress.StartTask();
			try {
				var counter = 0.0;
				func(progress, counter);
			} finally {
				progress.Value = progress.MaxValue;
				progress.StopTask();
			}
		}
	}
}