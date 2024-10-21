using Albatross.Dates;
using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace Sample.Proxy {
	public partial class RunProxyService : ClientBase {
		public RunProxyService(ILogger<RunProxyService> logger, HttpClient client) : base(logger, client) {
		}

		public const string ControllerPath = "api/run";
		public async Task Subscribe(System.String topic) {
			string path = $"{ControllerPath}/sub";
			var queryString = new NameValueCollection();
			queryString.Add("topic", topic);
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task Unsubscribe(System.String topic) {
			string path = $"{ControllerPath}/unsub";
			var queryString = new NameValueCollection();
			queryString.Add("topic", topic);
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}

		public async Task UnsubscribeAll() {
			string path = $"{ControllerPath}/unsub-all";
			var queryString = new NameValueCollection();
			using (var request = this.CreateRequest(HttpMethod.Post, path, queryString)) {
				await this.GetRawResponse(request);
			}
		}
	}
}
#nullable disable

