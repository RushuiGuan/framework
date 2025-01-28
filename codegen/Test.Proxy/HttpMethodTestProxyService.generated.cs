using Albatross.Dates;
using Albatross.Serialization;
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
		public async Task Delete() {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Delete, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task Post() {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task Patch() {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Patch, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task<System.Int32> Get() {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRequiredJsonResponseForValueType<System.Int32>(request);
			}
		}

		public async Task Put() {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Put, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

