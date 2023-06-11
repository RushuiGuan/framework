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

		#region utility
		protected HttpMethod GetPatchMethod() {
			return new HttpMethod("Patch");
		}
		public Uri BaseUrl => this.client.BaseAddress ?? throw new InvalidOperationException($"Base address not set for {this.GetType().FullName}");
		public string SerializeJson<T>(T t) => JsonSerializer.Serialize<T>(t, defaultSerializationOptions);
		public T? Deserialize<T>(string content) => JsonSerializer.Deserialize<T>(content, defaultSerializationOptions);
		public ValueTask<T?> DeserializeAsync<T>(Stream stream) => JsonSerializer.DeserializeAsync<T>(stream, defaultSerializationOptions);

		protected virtual JsonSerializerOptions defaultSerializationOptions => new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
		};
		#endregion

		void WriteRawResponse(TextWriter writer, HttpStatusCode statusCode, HttpHeaders headers, string content) {
			writer.WriteLine($"status-code: {statusCode}({(int)statusCode})");
			WriteHeader(writer, headers);
			writer.Write(content);
		}

		void WriteHeader(TextWriter myWriter, HttpHeaders headers) {
			foreach (var header in headers) {
				myWriter.Write(header.Key);
				myWriter.Write(":");
				foreach (var value in header.Value) {
					myWriter.Write(" ");
					myWriter.Write(value);
				}
				myWriter.WriteLine();
			}
		}

		void WriteRequest(HttpRequestMessage request) {
			if (writer != null) {
				writer.Write(request.Method);
				writer.Write(" ");
				writer.WriteLine(request.RequestUri);
				WriteHeader(writer, request.Headers);
				if (request.Content != null) {
					writer.WriteLine(request.Content.ReadAsStringAsync().Result);
				}
			}
		}

		#region creating request and response
		[Obsolete]
		public IEnumerable<HttpRequestMessage> CreateRequests(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, int maxUrlLength, 
			string arrayQueryKey, params string[] arrayQueryValues) {
			var urls = this.CreateRequestUrls(relativeUrl, queryStringValues, maxUrlLength, arrayQueryKey, arrayQueryValues);
			return urls.Select(args => new HttpRequestMessage(method, args)).ToArray();
		}
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
			request.Headers.CacheControl = new CacheControlHeaderValue() { NoCache = true };
			WriteRequest(request);
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

		public async Task<string> GetRawResponse(HttpRequestMessage request) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await client.SendAsync(request)) {
				return await GetRawResponse(response);
			}
		}
		public async Task<string> GetRawResponse(HttpResponseMessage response) {
			string content = await response.Content.ReadAsStringAsync();
			if (writer != null) {
				WriteRawResponse(writer, response.StatusCode, response.Headers, content);
			}
			EnsureStatusCode(response.StatusCode, response.RequestMessage.Method, response.RequestMessage.RequestUri, content);
			return content;
		}
		public async Task<string> GetRawResponse<ErrorType>(HttpRequestMessage request) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");

			using (var response = await client.SendAsync(request)) {
				return await GetRawResponse<ErrorType>(response);
			}
		}
		public async Task<string> GetRawResponse<ErrorType>(HttpResponseMessage response) {
			string content = await response.Content.ReadAsStringAsync();
			if (writer != null) {
				WriteRawResponse(writer, response.StatusCode, response.Headers, content);
			}
			EnsureStatusCode<ErrorType>(response.StatusCode, response.RequestMessage.Method, response.RequestMessage.RequestUri, content);
			return content;
		}
		public async Task<ResultType?> GetJsonResponse<ResultType, ErrorType>(HttpRequestMessage request) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await client.SendAsync(request)) {
				return await GetJsonResponse<ResultType, ErrorType>(response);
			}
		}
		public async Task<ResultType?> GetJsonResponse<ResultType, ErrorType>(HttpResponseMessage response) {
			string content = await response.Content.ReadAsStringAsync();
			if (writer != null) {
				WriteRawResponse(writer, response.StatusCode, response.Headers, content);
			}
			EnsureStatusCode<ErrorType>(response.StatusCode, response.RequestMessage.Method, response.RequestMessage.RequestUri, content);
			return response.StatusCode == HttpStatusCode.NoContent ? default(ResultType): Deserialize<ResultType>(content);
		}
		public async Task<ResultType?> GetJsonResponse<ResultType>(HttpRequestMessage request) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await client.SendAsync(request)) {
				return await GetJsonResponse<ResultType>(response);
			}
		}
		public async Task<ResultType?> GetJsonResponse<ResultType>(HttpResponseMessage response) {
			string content = await response.Content.ReadAsStringAsync();
			if (writer != null) {
				WriteRawResponse(writer, response.StatusCode, response.Headers, content);
			}
			EnsureStatusCode(response.StatusCode, response.RequestMessage.Method, response.RequestMessage.RequestUri, content);
			return response.StatusCode == HttpStatusCode.NoContent ? default(ResultType) : Deserialize<ResultType>(content);
		}
		public async Task Download<ErrorType>(HttpRequestMessage request, Stream stream) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await client.SendAsync(request)) {
				await this.Download<ErrorType>(response, stream);
			}
		}
		public async Task Download<ErrorType>(HttpResponseMessage response, Stream stream) {
			if (response.StatusCode != HttpStatusCode.OK) {
				string content = await response.Content.ReadAsStringAsync();
				if (writer != null) {
					WriteRawResponse(writer, response.StatusCode, response.Headers, content);
				}
				EnsureStatusCode<ErrorType>(response.StatusCode, response.RequestMessage.Method, response.RequestMessage.RequestUri, content);
			} else {
				await response.Content.CopyToAsync(stream);
			}
		}
		public async Task Download(HttpRequestMessage request, Stream stream) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await client.SendAsync(request)) {
				await this.Download(response, stream);
			}
		}
		public async Task Download(HttpResponseMessage response, Stream stream) {
			if (response.StatusCode != HttpStatusCode.OK) {
				string content = await response.Content.ReadAsStringAsync();
				if (writer != null) {
					WriteRawResponse(writer, response.StatusCode, response.Headers, content);
				}
				EnsureStatusCode(response.StatusCode, response.RequestMessage.Method, response.RequestMessage.RequestUri, content);
			} else {
				await response.Content.CopyToAsync(stream);
			}
		}

		public void EnsureStatusCode(HttpStatusCode statusCode, HttpMethod method, Uri endpoint, string content) {
			EnsureStatusCode<ServiceError>(statusCode, method, endpoint, content);
		}

		public void EnsureStatusCode<ErrorType>(HttpStatusCode statusCode, HttpMethod method, Uri endpoint, string content) {
			Exception exception;
			if ((int)statusCode > 399) {
				try {
					var error = Deserialize<ErrorType>(content);
					if (typeof(ErrorType) == typeof(ServiceError)) {
						exception = new ServiceException(statusCode, method, endpoint, error as ServiceError, content);
					} else {
						exception = new ServiceException<ErrorType>(statusCode, method, endpoint, error, content);
					}
				} catch {
					exception = new ServiceException(statusCode, method, endpoint, null, content);
				}
				throw exception;
			}
		}
		#endregion
	}
}