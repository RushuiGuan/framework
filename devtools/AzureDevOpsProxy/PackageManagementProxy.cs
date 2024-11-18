using Albatross.WebClient;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;

namespace AzureDevOpsProxy {
	public class PackageManagementProxy : Albatross.WebClient.ClientBase {
		private readonly AzureDevOpsProxyConfig config;

		public PackageManagementProxy(AzureDevOpsProxyConfig config, ILogger logger, HttpClient client) : base(logger, client) {
			this.config = config;
		}
		public async Task<Package[]> GetPackages(string? project, string feedId) {
			string relativeUrl = string.Empty;
			if (!string.IsNullOrEmpty(project)) {
				relativeUrl = $"{project}/";
			}
			relativeUrl += $"_apis/packaging/feeds/{feedId}/packages";
			var queryString = new NameValueCollection {
				{ "api-version", config.ApiVersion }
			};
			using var request = base.CreateRequest(HttpMethod.Get, relativeUrl, queryString);
			var result = await this.GetRequiredJsonResponse<ItemCollection<Package>>(request);
			return result.Value;
		}
		public async Task<PackageVersion[]> GetPackageVersions(string? project, string feedId, string packageId) {
			string relativeUrl = string.Empty;
			if (!string.IsNullOrEmpty(project)) {
				relativeUrl = $"{project}/";
			}
			relativeUrl += $"_apis/packaging/feeds/{feedId}/packages/{packageId}/versions";
			var queryString = new NameValueCollection {
				{ "api-version", config.ApiVersion },
				{ "isDeleted", "false" }
			};
			using var request = base.CreateRequest(HttpMethod.Get, relativeUrl, queryString);
			var result = await this.GetRequiredJsonResponse<ItemCollection<PackageVersion>>(request);
			return result.Value;
		}
		public async Task<string> GetPackageVersionsRaw(string? project, string feedId, string packageId) {
			string relativeUrl = string.Empty;
			if (!string.IsNullOrEmpty(project)) {
				relativeUrl = $"{project}/";
			}
			relativeUrl += $"_apis/packaging/feeds/{feedId}/packages/{packageId}/versions";
			var queryString = new NameValueCollection {
				{ "api-version", config.ApiVersion },
				{ "isDeleted", "false" }
			};
			using var request = base.CreateRequest(HttpMethod.Get, relativeUrl, queryString);
			return await this.GetRawResponse(request);
		}
	}
}
