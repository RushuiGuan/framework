using Microsoft.Extensions.Logging;
using Polly.Retry;
using Polly;
using System;
using System.Net.Http;
using System.Net;

namespace Albatross.RestClient {
	public static class RetryExtensions {
		public static bool ShouldRetry(Exception err, bool includeInternalServerError) {
			if (err is HttpRequestException) {
				return true;
			} else if (err is ServiceException serviceException) {
				return includeInternalServerError && serviceException.StatusCode == HttpStatusCode.InternalServerError
					|| serviceException.StatusCode == HttpStatusCode.RequestTimeout
					|| serviceException.StatusCode == HttpStatusCode.TooManyRequests
					|| serviceException.StatusCode == HttpStatusCode.ServiceUnavailable
					|| serviceException.StatusCode == HttpStatusCode.GatewayTimeout;
			} else {
				return false;
			}
		}
		/// <summary>
		/// retry with a exponential fallback of 1, 2, 4, 8, 16.. seconds
		/// use the maxDelayInSeconds to flatline the delay
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="predicate"></param>
		/// <param name="onRetry"></param>
		/// <param name="retryInternalServerError"></param>
		/// <param name="count">Determine the number of the retries</param>
		/// <param name="maxDelayInSeconds"></param>
		/// <returns></returns>
		public static AsyncRetryPolicy<T> GetDefaultRetryPolicy<T>(Func<T, bool> predicate, Action<DelegateResult<T>, TimeSpan> onRetry, bool retryInternalServerError, int count, int? maxDelayInSeconds, ILogger logger) {
			var array = new TimeSpan[count];
			var delay = 1;
			for (int i = 0; i < count; i++) {
				array[i] = TimeSpan.FromSeconds(delay);
				delay = delay * 2;
				if (delay > maxDelayInSeconds) {
					delay = maxDelayInSeconds.Value;
				}
			}
			logger.LogDebug("Setting up retry policy using the following step back sequence: {@array}", array);
			return Policy.Handle<Exception>(err => ShouldRetry(err, retryInternalServerError)).OrResult<T>(predicate)
					.WaitAndRetryAsync(array, (delegateResult, timespan) => onRetry(delegateResult, timespan));
		}
		public static AsyncRetryPolicy<T> GetDefaultRetryPolicy<T>(Func<T, bool> predicate, string what, bool retryInternalServerError, int count, int? maxDelayInSeconds, ILogger logger)
			=> GetDefaultRetryPolicy<T>(predicate, (delegateResult, timeSpan) => {
				logger.LogWarning("Retrying {what} after {timespan} seconds\non result: {@result}\nfor error: {error}",
					what, timeSpan, delegateResult.Result, delegateResult.Exception);
			}, retryInternalServerError, count, maxDelayInSeconds, logger);
	}
}
