using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;

namespace AzureDevOpsProxy {
	public class PackageOperationProxy : Albatross.WebClient.ClientBase {
		private readonly AzureDevOpsProxyConfig config;

		public PackageOperationProxy(AzureDevOpsProxyConfig config, ILogger logger, HttpClient client) : base(logger, client) {
			this.config = config;
		}

		public async Task DeleteNugetPackageVersion(string feedId, string packageName, string version) {
			var relativeUrl = $"_apis/packaging/feeds/{feedId}/nuget/packages/{packageName}/Versions/{version}";
			var queryString = new NameValueCollection {
				{ "api-version", config.ApiVersion },
			};
			using var request = base.CreateRequest(HttpMethod.Delete, relativeUrl, queryString);
			await this.GetRawResponse(request);
		}
	}
}
