using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class HttpMethodTestProxyService : ClientBase {
		public HttpMethodTestProxyService(ILogger<HttpMethodTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/http-method-test";
		public Task Delete() {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Delete, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task Post() {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task Patch() {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Patch, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}

		public Task<System.Int32> Get() {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponseForValueType<System.Int32>(request);
			}
		}

		public Task Put() {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Put, path, queryString)) {
				return this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

