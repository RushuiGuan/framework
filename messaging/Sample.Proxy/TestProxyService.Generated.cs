using Albatross.Dates;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Sample.Proxy {
	public partial class TestProxyService : ClientBase {
		public TestProxyService(ILogger<TestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/test";
		public async Task<System.String> TestPostPlainTextToBody(System.String text) {
			string path = $"{ControllerPath}/plain-text-post";
			var queryString = new NameValueCollection();
			using (var request = this.CreateStringRequest(HttpMethod.Post, path, queryString, text)) {
				return await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

