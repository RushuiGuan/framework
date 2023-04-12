using Albatross.Config;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Framework.Utility {
	public static class Extensions {
		public static IServiceCollection AddTestProxy(this IServiceCollection services) {
			services.AddConfig<ClientConfig>();
			services.AddHttpClient("test")
				.AddTypedClient<TestHttpClientProxy>()
				.ConfigureHttpClient((provider, client) => {
					var config = provider.GetRequiredService<ClientConfig>();
					client.BaseAddress = new Uri(config.TestApiEndPoint);
					client.DefaultRequestHeaders.Add("test-header1", "test-header-value1");
					client.DefaultRequestHeaders.Add("test-header2", "test-header-value2");
				});
			return services;
		}
	}
}
