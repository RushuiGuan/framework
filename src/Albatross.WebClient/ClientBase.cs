using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Polly;
using System.Linq;
using Polly.Retry;
using System.Data;
using System.IO.Compression;

namespace Albatross.WebClient {
	public abstract class ClientBase {
		protected HttpClient client;
		protected ILogger logger;
		private TextWriter? writer;

		public ClientBase(ILogger logger, HttpClient client) {
			this.client = client;
			this.logger = logger;
		}

		public void UseTextWriter(TextWriter writer) { this.writer = writer; }
		public virtual int MaxRedirect => 3;
		public const string GZipEncoding = "gzip";

		#region utility
		public Uri BaseUrl => this.client.BaseAddress ?? throw new InvalidOperationException($"Base address not set for {this.GetType().FullName}");
		public string SerializeJson<T>(T t) => JsonSerializer.Serialize<T>(t, defaultSerializationOptions);
		public T? Deserialize<T>(string content) => JsonSerializer.Deserialize<T>(content, defaultSerializationOptions);
		public ValueTask<T?> DeserializeAsync<T>(Stream stream) => JsonSerializer.DeserializeAsync<T>(stream, defaultSerializationOptions);
		protected virtual JsonSerializerOptions defaultSerializationOptions => new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
		};
		public async Task<HttpRequestMessage> CloneHttpRequest(HttpRequestMessage request, Uri? updatedUri) {
			var result = new HttpRequestMessage(request.Method, updatedUri ?? request.RequestUri) {
				Version = request.Version
			};
			if (request.Content != null) {
				var ms = new MemoryStream();
				await request.Content.CopyToAsync(ms);
				ms.Position = 0;
				result.Content = new StreamContent(ms);
				foreach (var item in request.Content.Headers) {
					result.Content.Headers.TryAddWithoutValidation(item.Key, item.Value);
				}
			}
			return result;
		}
		public static bool ShouldRedirect(HttpResponseMessage response) {
			switch (response.StatusCode) {
				case HttpStatusCode.MultipleChoices:
				case HttpStatusCode.Moved:
				case HttpStatusCode.Found:
				case HttpStatusCode.SeeOther:
				case HttpStatusCode.TemporaryRedirect:
				case HttpStatusCode.PermanentRedirect:
					return true;
				default:
					return false;
			}
		}
		#endregion

		#region retry logic 
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
		public AsyncRetryPolicy<T> GetDefaultRetryPolicy<T>(Func<T, bool> predicate, Action<DelegateResult<T>, TimeSpan> onRetry, bool retryInternalServerError, int count, int? maxDelayInSeconds) {
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
		public AsyncRetryPolicy<T> GetDefaultRetryPolicy<T>(Func<T, bool> predicate, string what, bool retryInternalServerError, int count, int? maxDelayInSeconds)
			=> this.GetDefaultRetryPolicy<T>(predicate, (delegateResult, timeSpan) => {
				this.logger.LogWarning("Retrying {what} after {timespan} seconds\non result: {@result}\nfor error: {error}",
					what, timeSpan, delegateResult.Result, delegateResult.Exception);
			}, retryInternalServerError, count, maxDelayInSeconds);
		#endregion

		#region creating request
		public IEnumerable<string> CreateRequestUrls(string relativeUrl, NameValueCollection queryStringValues, int maxUrlLength, string arrayQueryKey, params string[] arrayQueryValues) {
			List<string> urls = new List<string>();
			int offset = 0;
			do {
				var sb = relativeUrl.CreateUrl(queryStringValues);
				int index;
				for (index = offset; index < arrayQueryValues.Length; index++) {
					int current = sb.Length;
					sb.AddQueryParam(arrayQueryKey, arrayQueryValues[index]!);
					if (sb.Length > maxUrlLength - this.BaseUrl.AbsoluteUri.Length) {
						sb.Length = current;
						if (index == 0) {
							throw new InvalidOperationException("Cannot create requests because url max length is smaller than the minimum length required for a single request");
						}
						break;
					}
				}
				urls.Add(sb.ToString());
				offset = index;
			} while (offset < arrayQueryValues.Length);
			return urls;
		}
		public HttpRequestMessage CreateRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues) {
			var request = new HttpRequestMessage(method, relativeUrl.CreateUrl(queryStringValues).ToString());
			// NoCache has been moved to DefaultRequestHeaders during client setup
			// request.Headers.CacheControl = new CacheControlHeaderValue() { NoCache = true };
			writer.LogRequest(request, client);
			return request;
		}
		public HttpRequestMessage CreateJsonRequest<T>(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, T t) {
			var request = CreateRequest(method, relativeUrl, queryStringValues);
			string content = SerializeJson<T>(t);
			request.Content = new StringContent(content, Encoding.UTF8, Constant.JsonContentType);
			writer?.WriteLine(content);
			return request;
		}
		public HttpRequestMessage CreateStringRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, string content) {
			var request = CreateRequest(method, relativeUrl, queryStringValues);
			request.Content = new StringContent(content, Encoding.UTF8, Constant.TextHtmlContentType);
			writer?.WriteLine(content);
			return request;
		}
		public HttpRequestMessage CreateStreamRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, Stream stream) {
			var request = CreateRequest(method, relativeUrl, queryStringValues);
			request.Content = new StreamContent(stream);
			return request;
		}
		public HttpRequestMessage CreateMultiPartFormRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, params MultiPartFormData[] formDataArray) {
			var request = CreateRequest(method, relativeUrl, queryStringValues);
			var content = new MultipartFormDataContent();
			foreach (var item in formDataArray) {
				content.AddMultiPartFormData(item);
			}
			request.Content = content;
			return request;
		}
		#endregion

		#region get response
		public async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request, int redirectCount = 0) {
			var response = await client.SendAsync(request);
			if (ShouldRedirect(response)) {
				if (redirectCount > MaxRedirect) {
					throw new InvalidOperationException($"Max redirect count of {MaxRedirect} exceeded");
				}
				Uri redirectUri = response.Headers.Location;
				if (!redirectUri.IsAbsoluteUri) {
					redirectUri = new Uri(response.RequestMessage.RequestUri, response.Headers.Location);
				}
				response.Dispose();
				using (var newRequest = await CloneHttpRequest(response.RequestMessage, redirectUri)) {
					logger.LogInformation("Redirected from {url} to {to}", request.RequestUri, newRequest.RequestUri);
					return await SendRequest(newRequest, redirectCount++);
				}
			} else {
				return response;
			}
		}
		public async Task<string> GetRawResponse(HttpRequestMessage request) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await SendRequest(request)) {
				return await ProcessResponseAsText(response);
			}
		}
		public async Task<string> GetRawResponse<ErrorType>(HttpRequestMessage request) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await SendRequest(request)) {
				return await ProcessResponseAsText<ErrorType>(response);
			}
		}
		public async Task<ResultType?> GetJsonResponse<ResultType, ErrorType>(HttpRequestMessage request) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await SendRequest(request)) {
				return await ProcessResponseAsJson<ResultType, ErrorType>(response);
			}
		}
		public async Task<ResultType?> GetJsonResponse<ResultType>(HttpRequestMessage request) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await SendRequest(request)) {
				return await ProcessResponseAsJson<ResultType>(response);
			}
		}
		public async Task Download<ErrorType>(HttpRequestMessage request, Stream stream) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await SendRequest(request)) {
				await this.Download<ErrorType>(response, stream);
			}
		}
		public async Task Download(HttpRequestMessage request, Stream stream) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await SendRequest(request)) {
				await this.Download(response, stream);
			}
		}
		#endregion

		#region error processing
		public Task EnsureStatusCode(HttpResponseMessage response) => EnsureStatusCode<ServiceError>(response);
		public async Task EnsureStatusCode<ErrorType>(HttpResponseMessage response) {
			Exception exception;
			if ((int)response.StatusCode > 399) {
				string content = await response.ReadResponseAsText();
				try {
					var error = Deserialize<ErrorType>(content);
					if (typeof(ErrorType) == typeof(ServiceError)) {
						exception = new ServiceException(response.StatusCode, response.RequestMessage.Method, response.RequestMessage.RequestUri, error as ServiceError, content);
					} else {
						exception = new ServiceException<ErrorType>(response.StatusCode, response.RequestMessage.Method, response.RequestMessage.RequestUri, error, content);
					}
				} catch {
					exception = new ServiceException(response.StatusCode, response.RequestMessage.Method, response.RequestMessage.RequestUri, null, content);
				}
				throw exception;
			}
		}
		#endregion

		#region process response
		public async Task<T?> ReadResponseAsJson<T>(HttpResponseMessage response) {
			if (response.StatusCode == HttpStatusCode.NoContent || response.Content.Headers.ContentLength == 0) {
				return default;
			} else {
				var stream = await response.Content.ReadAsStreamAsync();
				if (response.Content.Headers.ContentEncoding.Contains(GZipEncoding)) {
					var gzip = new GZipStream(stream, CompressionMode.Decompress);
					return await DeserializeAsync<T>(gzip);
				} else {
					return await DeserializeAsync<T>(stream);
				}
			}
		}
		public async Task<string> ProcessResponseAsText(HttpResponseMessage response) {
			try {
				await EnsureStatusCode(response);
				string content = await response.ReadResponseAsText();
				return content;
			} finally {
				await writer.LogResponse(response);
			}
		}
		public async Task<string> ProcessResponseAsText<ErrorType>(HttpResponseMessage response) {
			try {
				await EnsureStatusCode<ErrorType>(response);
				string content = await response.ReadResponseAsText();
				return content;
			} finally {
				await writer.LogResponse(response);
			}
		}
		public async Task<ResultType?> ProcessResponseAsJson<ResultType, ErrorType>(HttpResponseMessage response) {
			try {
				await EnsureStatusCode<ErrorType>(response);
				var result = await ReadResponseAsJson<ResultType>(response);
				return result;
			} finally {
				await writer.LogResponse(response);
			}
		}
		public async Task<ResultType?> ProcessResponseAsJson<ResultType>(HttpResponseMessage response) {
			try {
				await EnsureStatusCode(response);
				string content = await response.ReadResponseAsText();
				return response.StatusCode == HttpStatusCode.NoContent ? default(ResultType) : Deserialize<ResultType>(content);
			} finally {
				await writer.LogResponse(response);
			}
		}
		public async Task Download<ErrorType>(HttpResponseMessage response, Stream stream) {
			try {
				await EnsureStatusCode<ErrorType>(response);
				if (response.Content.Headers.ContentEncoding.Contains(ClientBase.GZipEncoding)) {
					using var responseStream = await response.Content.ReadAsStreamAsync();
					using var gzip = new GZipStream(responseStream, CompressionMode.Decompress);
					await gzip.CopyToAsync(stream);
				} else {
					await response.Content.CopyToAsync(stream);
				}
			} finally {
				await writer.LogResponse(response, false);
			}
		}
		public Task Download(HttpResponseMessage response, Stream stream) => Download<ServiceError>(response, stream);
		#endregion
	}
}