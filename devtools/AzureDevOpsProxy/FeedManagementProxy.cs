using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;

namespace AzureDevOpsProxy {
	public class FeedManagementProxy : Albatross.WebClient.ClientBase {
		private readonly AzureDevOpsProxyConfig config;

		public FeedManagementProxy(AzureDevOpsProxyConfig config, ILogger logger, HttpClient client) : base(logger, client) {
			this.config = config;
		}
		public async Task<Feed[]> GetFeeds(string? project) {
			string relativeUrl = string.Empty;
			if (!string.IsNullOrEmpty(project)) {
				relativeUrl = $"{project}/";
			}
			relativeUrl += $"_apis/packaging/feeds";
			var queryString = new NameValueCollection {
				{ "api-version", config.ApiVersion }
			};
			using var request = base.CreateRequest(HttpMethod.Get, relativeUrl, queryString);
			var result = await this.GetRequiredJsonResponse<ItemCollection<Feed>>(request);
			return result.Value;
		}
	}
}