using Albatross.Config;
using Microsoft.Extensions.DependencyInjection;

namespace AzureDevOpsProxy {
	public static class Extensions {
		public static IServiceCollection AddAzureDevOpsProxy(this IServiceCollection services) {
			services.AddConfig<AzureDevOpsProxyConfig>();
			services.AddHttpClient("azure-devops-mgmt")
				.AddTypedClient<FeedManagementProxy>()
				.AddTypedClient<PackageManagementProxy>()
				.ConfigureHttpClient((provider, client) => {
					var cfg = provider.GetRequiredService<AzureDevOpsProxyConfig>();
					client.BaseAddress = new Uri(cfg.FeedsBaseUrl);
					client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", cfg.BasicAuth);
				});
			services.AddHttpClient("azure-devops-ops")
				.AddTypedClient<PackageOperationProxy>()
				.ConfigureHttpClient((provider, client) => {
					var cfg = provider.GetRequiredService<AzureDevOpsProxyConfig>();
					client.BaseAddress = new Uri(cfg.PackagesBaseUrl);
					client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", cfg.BasicAuth);
				});
			return services;
		}
	}
}
