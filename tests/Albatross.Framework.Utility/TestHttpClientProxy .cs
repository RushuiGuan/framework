using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Albatross.Framework.Utility {
	public class MyResult {
		public MyResult(string name) {
			this.Name = name;
		}
		public string Name { get; set; }
	}
	public class TestHttpClientProxy : ClientBase {
		public const System.String ControllerPath = "api/client-test";

		public TestHttpClientProxy(ILogger logger, HttpClient client) : base(logger, client) {
		}

		public async Task<string> GetStringResponse(string name) {
			string path = $"{ControllerPath}/string-response";
			var queryString = new System.Collections.Specialized.NameValueCollection {
				{ "name", name }
			};
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
		public async Task<string> GetTextResponse(string name) {
			string path = $"{ControllerPath}/text-response";
			var queryString = new System.Collections.Specialized.NameValueCollection {
				{ "name", name }
			};
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
		public async Task<MyResult?> GetJsonResponse(string name) {
			string path = $"{ControllerPath}/json-response";
			var queryString = new System.Collections.Specialized.NameValueCollection {
				{ "name", name }
			};
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<MyResult>(request);
			}
		}
		public async Task<MyResult?> StandardError(string name) {
			string path = $"{ControllerPath}/standard-error";
			var queryString = new System.Collections.Specialized.NameValueCollection {
				{ "name", name }
			};
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<MyResult>(request);
			}
		}

		public async Task<MyResult?> CustomError(string name) {
			string path = $"{ControllerPath}/custom-error";
			var queryString = new System.Collections.Specialized.NameValueCollection {
				{ "name", name }
			};
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<MyResult, MyResult>(request);
			}
		}

		public async Task<object?> NoContent1(string name) {
			string path = $"{ControllerPath}/no-response-1";
			var queryString = new System.Collections.Specialized.NameValueCollection {
				{ "name", name }
			};
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetJsonResponse<MyResult, MyResult>(request);
			}
		}

		public async Task<object?> NoContent2(string name) {
			string path = $"{ControllerPath}/no-response-2";
			var queryString = new System.Collections.Specialized.NameValueCollection {
				{ "name", name }
			};
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
	}
}
