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

namespace Albatross.WebClient {
	public abstract class ClientBase {
		protected HttpClient client;
		protected ILogger logger;
		private TextWriter writer;

		public ClientBase(ILogger logger, HttpClient client) {
			this.client = client;
			this.logger = logger;
		}

		public void UseTextWriter(TextWriter writer) { this.writer = writer; }

		#region utility
		protected HttpMethod GetPatchMethod() {
			return new HttpMethod("Patch");
		}
		public Uri BaseUrl => this.client.BaseAddress;
		public string SerializeJson<T>(T t) => JsonSerializer.Serialize<T>(t, defaultSerializationOptions);
		public T Deserialize<T>(string content) => JsonSerializer.Deserialize<T>(content, defaultSerializationOptions);
		public ValueTask<T> DeserializeAsync<T>(Stream stream) => JsonSerializer.DeserializeAsync<T>(stream, defaultSerializationOptions);

		protected virtual JsonSerializerOptions defaultSerializationOptions => new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			IgnoreNullValues = true,
		};
		#endregion

		void WriteHeader(HttpHeaders headers) {
			foreach (var header in headers) {
				writer.Write(header.Key);
				writer.Write(":");
				foreach (var value in header.Value) {
					writer.Write(" ");
					writer.Write(value);
				}
				writer.WriteLine();
			}
		}

		void WriteRequest(HttpRequestMessage request) {
			if (writer != null) {
				writer.Write(request.Method);
				writer.Write(" ");
				writer.WriteLine(request.RequestUri);
				WriteHeader(request.Headers);
				if (request.Content != null) {
					writer.WriteLine(request.Content.ReadAsStringAsync().Result);
				}
			}
		}

		#region creating request and response
		public HttpRequestMessage CreateRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues) {
			var request = new HttpRequestMessage(method, relativeUrl.GetUrl(queryStringValues));
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
		public async Task<string> Invoke(HttpRequestMessage request, Func<HttpStatusCode, string, Exception> throwCustomException = null) {
			logger.LogInformation("{method}: {url}", request.Method, $"{new Uri(client.BaseAddress, request.RequestUri)}");
			using (var response = await client.SendAsync(request)) {
				string content = await response.Content.ReadAsStringAsync();
				if (writer != null) {
					WriteHeader(response.Headers);
					writer.Write(content);
				}
				if (response.IsSuccessStatusCode) {
					return content;
				} else {
					if (throwCustomException == null) {
						ErrorMessage error;
						if (string.IsNullOrEmpty(content)) {
							error = new ErrorMessage() {
								StatusCode = response.StatusCode,
							};
						} else {
							try {
								error = Deserialize<ErrorMessage>(content);
							} catch {
								error = new ErrorMessage();
								error.Message = content;
							}
							error.StatusCode = response.StatusCode;
						}
						throw new ClientException(error);
					} else {
						throw throwCustomException(response.StatusCode, content);
					}
				}
			}
		}
		public async Task<T> Invoke<T>(HttpRequestMessage request, Func<HttpStatusCode, string, Exception> throwCustomException = null) {
			string content = await Invoke(request, throwCustomException);
			return Deserialize<T>(content);
		}
		#endregion
	}
}