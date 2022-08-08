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
		public IEnumerable<HttpRequestMessage> CreateRequests(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues,int maxUrlLength, string arrayQueryKey, params string[] arrayQueryValues) {
			List<HttpRequestMessage> requests = new List<HttpRequestMessage>();
			int offset = 0;
			do {
				var sb = relativeUrl.CreateUrl(queryStringValues);
#pragma warning disable CS1717 // Assignment made to same variable
				for (offset = offset; offset < arrayQueryValues.Length; offset++) {
					int current = sb.Length;
					sb.AddQueryParam(arrayQueryKey, arrayQueryValues[offset]!);
					if (sb.Length > maxUrlLength - this.BaseUrl.AbsoluteUri.Length) {
						sb.Length = current;
						break;
					}
				}
#pragma warning restore CS1717 // Assignment made to same variable
				var request = new HttpRequestMessage(method, sb.ToString());
				requests.Add(request);
			} while (offset < arrayQueryValues.Length);
			return requests;
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
		public HttpRequestMessage CreateMultiPartFormRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, string boundary, Stream stream) {
			var request = CreateRequest(method, relativeUrl, queryStringValues);
			var formData = new MultipartFormDataContent(boundary);
			formData.Add(new StreamContent(stream));
			request.Content = formData;
			return request;
		}

		public async Task<string> Invoke(HttpRequestMessage request, Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await client.SendAsync(request)) {
				await EnsureStatusCode(response, throwCustomException);
				string content = await response.Content.ReadAsStringAsync();
				if (writer != null) {
					WriteHeader(writer, response.Headers);
					writer.Write(content);
				}
				return content;
			}
		}
		public async Task<T?> Invoke<T>(HttpRequestMessage request, Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			string content = await Invoke(request, throwCustomException);
			return Deserialize<T>(content);
		}

		public async Task Download(HttpRequestMessage request, Stream stream, Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			logger.LogDebug("{method}: {url}", request.Method, $"{new Uri(BaseUrl, request.RequestUri!)}");
			using (var response = await client.SendAsync(request)) {
				await EnsureStatusCode(response, throwCustomException);
				await response.Content.CopyToAsync(stream);
			}
		}

		public async Task EnsureStatusCode(HttpResponseMessage response, Func<HttpStatusCode, string, Exception>? throwCustomException = null) {
			if(response.StatusCode != HttpStatusCode.OK) {
				string content = await response.Content.ReadAsStringAsync();
				if (throwCustomException == null) {
					ErrorMessage error;
					try {
						error = Deserialize<ErrorMessage>(content) ?? new ErrorMessage();
					} catch {
						error = new ErrorMessage();
						error.Message = content;
					}
					error.StatusCode = response.StatusCode;
					throw new ClientException(error);
				} else {
					throw throwCustomException(response.StatusCode, content);
				}
			}
		}
		#endregion
	}
}