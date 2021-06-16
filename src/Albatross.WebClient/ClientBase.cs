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

		public ClientBase(ILogger logger, HttpClient client) {
			this.client = client;
			this.logger = logger;
        }

        #region utility
        protected HttpMethod GetPatchMethod() {
			return new HttpMethod("Patch");
		}
		public Uri BaseUrl => this.client.BaseAddress;
		protected string SerializeJson<T>(T t) {
			return JsonSerializer.Serialize<T>(t, defaultSerializationOptions);
		}
		protected T Deserialize<T>(string content) {
			return JsonSerializer.Deserialize<T>(content, defaultSerializationOptions);
		}
		protected virtual JsonSerializerOptions defaultSerializationOptions => new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			IgnoreNullValues = true,
		};
		#endregion

		#region creating request and response
		public HttpRequestMessage CreateRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues) {
			var request = new HttpRequestMessage(method, relativeUrl.GetUrl(queryStringValues));
			request.Headers.CacheControl = new CacheControlHeaderValue() { NoCache = true };
			return request;
		}
		public HttpRequestMessage CreateJsonRequest<T>(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, T t, StreamWriter logger) {
			var request = CreateRequest(method, relativeUrl, queryStringValues);
			string content = SerializeJson<T>(t);
			logger?.Write(content);
			request.Content = new StringContent(content, Encoding.UTF8, Constant.JsonContentType);
			//request.Headers.Add(Constant.ContentType, Constant.JsonContentType);
			return request;
		}
		public HttpRequestMessage CreateStringRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, string content) {
			var request = CreateRequest(method, relativeUrl, queryStringValues);
			request.Content = new StringContent(content, Encoding.UTF8, Constant.TextHtmlContentType);
			return request;
		}
		public async Task<string> Invoke(HttpRequestMessage request, Func<HttpStatusCode, string, Exception> throwCustomException = null) {
			logger.LogInformation("{method}: {url}", request.Method, $"{new Uri(client.BaseAddress, request.RequestUri)}");
			using (var response = await client.SendAsync(request)) {
				string content = await response.Content.ReadAsStringAsync();
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