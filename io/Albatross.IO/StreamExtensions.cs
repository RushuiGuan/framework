using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Polly.Retry;

namespace Albatross.IO {
	public static class StreamExtensions {
		public static Task<Stream> OpenSharedReadStreamWithRetry(this FileInfo file, int bufferSize, int retryCount, int delay_ms, FileOptions fileOptions, ILogger logger, Action<int>? onRetry = null, CancellationToken? cancellationToken = null) {
			var policy = CreateRetryPolicy(retryCount, delay_ms, onRetry, logger);
			var context = new Context(file.FullName);
			context["action"] = "open-async-shared-read";
			return policy.ExecuteAsync((context, token) => {
				Stream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, fileOptions);
				return Task.FromResult(stream);
			}, context, cancellationToken ?? CancellationToken.None);
		}

		public static Task<Stream> OpenExclusiveReadWriteStreamWithRetry(this FileInfo file, int bufferSize, int retryCount, int delay_ms, FileOptions fileOptions, ILogger logger, Action<int>? onRetry = null, CancellationToken? cancellationToken = null) {
			var policy = CreateRetryPolicy(retryCount, delay_ms, onRetry, logger);
			var context = new Context(file.FullName);
			context["action"] = "open-async-exclusive-readwrite";
			return policy.ExecuteAsync((context, token) => {
				Stream stream = new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, bufferSize, fileOptions);
				return Task.FromResult(stream);
			}, context, cancellationToken ?? CancellationToken.None);
		}

		const int ERROR_SHARING_VIOLATION = unchecked((int)0x80070020);
		static AsyncRetryPolicy<Stream> CreateRetryPolicy(int count, int delay_ms, Action<int>? action, ILogger logger) {
			return Policy.Handle<IOException>(err => err.HResult == ERROR_SHARING_VIOLATION)
				.OrResult<Stream>(x => false)
				.WaitAndRetryAsync<Stream>(count, x => TimeSpan.FromMilliseconds(delay_ms), (result, delay, count, context) => {
					if (action != null) {
						action(count);
					}
					logger.LogWarning("{count} retry to open file {name} for {action} after {delay:#,#}ms", count, context.OperationKey, context["action"], delay.Milliseconds);
				});
		}
	}
}
