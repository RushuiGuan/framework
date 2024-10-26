using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

namespace Test.Proxy {
	public partial class AbsUrlRedirectTestProxyService : ClientBase {
		public AbsUrlRedirectTestProxyService(ILogger<AbsUrlRedirectTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/abs-url-redirect-test";
		public async Task<string> Get(int actionId) {
			string path = $"{ControllerPath}/test-{actionId}";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
	}
}