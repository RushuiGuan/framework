using Albatross.Config;
using Albatross.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;

namespace Test.Proxy {
	public static partial class Extensions {
		public static IServiceCollection AddTestProxy(this IServiceCollection services) {
			services.AddConfig<TestProxyConfig>()
				.TryAddSingleton<IJsonSettings, DefaultJsonSettings>();
			services.AddHttpClient("test-proxy").AddClients()
				.AddTypedClient<RedirectTestProxyService>()
				.AddTypedClient<AbsUrlRedirectTestProxyService>()
				.ConfigureHttpClient((provider, client) => {
					var config = provider.GetRequiredService<TestProxyConfig>();
					client.BaseAddress = new Uri(config.EndPoint);
				}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {
					UseDefaultCredentials = true,
					AllowAutoRedirect = false,
				});
			return services;
		}
	}
}