using Albatross.Config;
using Albatross.Serialization;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Test.Proxy {
	public static partial class Extensions {
		public static IServiceCollection AddTestProxy(this IServiceCollection services) {
			services.AddConfig<TestProxyConfig>()
				.TryAddSingleton<IJsonSettings, DefaultJsonSettings>();
			services.AddHttpClient("test-proxy").AddClients()
				.ConfigureHttpClient((provider, client) => {
					var config = provider.GetRequiredService<TestProxyConfig>();
					client.BaseAddress = new Uri(config.EndPoint);
				}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {
					UseDefaultCredentials = true,
				});
			return services;
		}

		public static string ISO8601String(this DateOnly value) => $"{value:yyyy-MM-dd}";
		public static string ISO8601String(this TimeOnly value) => $"{value:HH:mm:ss.fffffff}";
	}
}
