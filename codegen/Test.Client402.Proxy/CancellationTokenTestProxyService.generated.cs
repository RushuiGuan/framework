using Albatross.Dates;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Test.Proxy {
	public partial class CancellationTokenTestProxyService : ClientBase {
		public CancellationTokenTestProxyService(ILogger<CancellationTokenTestProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/cancellationtokentest";
		public async Task<System.String> Get(System.Threading.CancellationToken cancellationToken) {
			string path = $"{ControllerPath}";
			var queryString = new NameValueCollection();
			queryString.Add("cancellationToken", $"{cancellationToken}");
			using (var request = this.CreateRequest(HttpMethod.Get, path, queryString)) {
				return await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

