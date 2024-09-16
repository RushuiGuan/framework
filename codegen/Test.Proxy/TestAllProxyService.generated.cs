using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class TestAllProxyService : ClientBase {
		public TestAllProxyService(ILogger<TestAllProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/testall";
		public Task<Test.Dto.MyDto> Get(System.String id, System.String name) {
			string path = $"{ControllerPath}/{id}";
			var queryString = new NameValueCollection();
			queryString.Add("name", name);
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return this.GetRequiredJsonResponse<Test.Dto.MyDto>(request);
			}
		}
	}
}
#nullable disable

