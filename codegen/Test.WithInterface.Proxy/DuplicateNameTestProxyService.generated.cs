using Albatross.Dates;
using Albatross.Serialization;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.WithInterface.Proxy {
	public partial interface IDuplicateNameTestProxyService {
		Task Submit(System.Int32 id);
		Task Submit(System.String name);
	}

	public partial class DuplicateNameTestProxyService : ClientBase, IDuplicateNameTestProxyService {
		public DuplicateNameTestProxyService(ILogger<DuplicateNameTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/duplicate-name-test";
		public async Task Submit(System.Int32 id) {
			string path = $"{ControllerPath}/by-id";
			var queryString = new NameValueCollection();
			queryString.Add("id", $"{id}");
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task Submit(System.String name) {
			string path = $"{ControllerPath}/by-name";
			var queryString = new NameValueCollection();
			queryString.Add("name", name);
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

