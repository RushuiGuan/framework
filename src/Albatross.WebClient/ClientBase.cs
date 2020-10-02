using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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
		};
		#endregion

		#region creating request and response
		public HttpRequestMessage CreateRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues) {
			var request = new HttpRequestMessage(method, relativeUrl.GetUrl(queryStringValues));
			request.Headers.CacheControl = new CacheControlHeaderValue() { NoCache = true };
			return request;
		}
		public HttpRequestMessage CreateJsonRequest<T>(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, T t) {
			var request = CreateRequest(method, relativeUrl, queryStringValues);
			string content = SerializeJson<T>(t);
			request.Content = new StringContent(content, Encoding.UTF8, Constant.JsonContentType);
			//request.Headers.Add(Constant.ContentType, Constant.JsonContentType);
			return request;
		}
		public HttpRequestMessage CreateStringRequest(HttpMethod method, string relativeUrl, NameValueCollection queryStringValues, string content) {
			var request = CreateRequest(method, relativeUrl, queryStringValues);
			request.Content = new StringContent(content);
			request.Headers.Add(Constant.ContentType, Constant.TextHtmlContentType);
			return request;
		}
		public async Task<string> Invoke(HttpRequestMessage request) {
			using (var response = await client.SendAsync(request)) {
				string content = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode) {
					return content;
				} else {
					if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) {
						ErrorMessage error;
						if (string.IsNullOrEmpty(content)) {
							error = new ErrorMessage() { Message = "An error has occurred", };
						} else {
							error = Deserialize<ErrorMessage>(content);
						}
						error.StatusCode = (int)response.StatusCode;
						error.StatusDescription = Convert.ToString(response.StatusCode);
						throw new ClientException(error);
					} else {
						response.EnsureSuccessStatusCode();
						throw new Exception();
					}
				}
			}
		}
		public async Task<T> Invoke<T>(HttpRequestMessage request) {
			string content = await Invoke(request);
			return Deserialize<T>(content);
		}
		#endregion
	}
}