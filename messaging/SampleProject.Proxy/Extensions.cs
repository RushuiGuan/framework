using Albatross.Config;
using Albatross.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;

namespace SampleProject.Proxy {
	public static class Extensions {
		public static IServiceCollection AddSampleProjectProxy(this IServiceCollection services) {
			services.AddConfig<SampleProjectProxyConfig>();
			services.AddHttpClient("sample-project")
				.AddTypedClient<RunProxyService>()
				.ConfigureHttpClient((provider, client) => {
					var config = provider.GetRequiredService<SampleProjectProxyConfig>();
					client.BaseAddress = new Uri(config.EndPoint);
				});
			return services;
		}
	}
}