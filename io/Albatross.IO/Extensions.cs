using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.IO {
	public static class Extensions {
		/// <summary>
		/// Check if the parent directory of the file exists.  If not, create the directory
		/// </summary>
		/// <param name="file">expected a valid file path</param>
		/// <exception cref="ArgumentException">throw the exception if the input file path is not valid</exception>
		public static void EnsureDirectory(this string file) {
			var directory = Path.GetDirectoryName(file);
			if (directory != null) {
				if (!Directory.Exists(directory)) {
					Directory.CreateDirectory(directory);
				}
			} else {
				throw new ArgumentException($"Path {file} is not valid");
			}
		}

		/// <summary>
		/// Convert the text to a valid file name by replacing invalid characters with the filler character
		/// </summary>
		/// <param name="text">the text that is used to create the file name</param>
		/// <param name="filler">the filler character used to replace the invalid characters</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">throws when the filler characters are not valid file name characters</exception>
		public static string ConvertToFilename(this string text, string filler) {
			var sb = new StringBuilder(text.Length);
			var invalidChars = Path.GetInvalidFileNameChars().ToHashSet();
			foreach (var character in filler) {
				if (invalidChars.Contains(character)) {
					throw new ArgumentException($"Filler character {character} is not a valid file name character");
				}
			}
			foreach (var character in text) {
				if (invalidChars.Contains(character)) {
					sb.Append(filler);
				} else {
					sb.Append(character);
				}
			}
			return sb.ToString();
		}

		public static Task<Stream> OpenAsyncSharedReadStreamWithRetry(this FileInfo file, int bufferSize, int retryCount, int delay_ms, ILogger logger, Action<int>? onRetry = null, CancellationToken? cancellationToken = null) {
			var policy = CreateRetryPolicy(retryCount, delay_ms, onRetry, logger);
			var context = new Context(file.FullName);
			context["action"] = "open-async-shared-read";
			return policy.ExecuteAsync((context, token) => {
				Stream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
				return Task.FromResult(stream);
			}, context, cancellationToken ?? CancellationToken.None);
		}

		public static Task<Stream> OpenAsyncExclusiveWriteStreamWithRetry(this FileInfo file, int bufferSize, int retryCount, int delay_ms, ILogger logger, Action<int>? onRetry = null, CancellationToken? cancellationToken = null) {
			var policy = CreateRetryPolicy(retryCount, delay_ms, onRetry, logger);
			var context = new Context(file.FullName);
			context["action"] = "open-async-exclusive-write";
			return policy.ExecuteAsync((context, token) => {
				Stream stream = new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize, FileOptions.Asynchronous);
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