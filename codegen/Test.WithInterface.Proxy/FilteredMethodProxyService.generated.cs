using Albatross.Dates;
using Albatross.Serialization;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.WithInterface.Proxy {
	public partial interface IFilteredMethodProxyService {
		Task FilteredByAll();
		Task FilteredByNone();
		Task FilteredByCSharp2();
		Task IncludedByCSharp();
		Task FilteredByTypeScript();
	}

	public partial class FilteredMethodProxyService : ClientBase, IFilteredMethodProxyService {
		public FilteredMethodProxyService(ILogger<FilteredMethodProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/filtered-method";
		public async Task FilteredByAll() {
			string path = $"{ControllerPath}/all";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task FilteredByNone() {
			string path = $"{ControllerPath}/none";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task FilteredByCSharp2() {
			string path = $"{ControllerPath}/csharp2";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task IncludedByCSharp() {
			string path = $"{ControllerPath}/include-this-method";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task FilteredByTypeScript() {
			string path = $"{ControllerPath}/typescript";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

