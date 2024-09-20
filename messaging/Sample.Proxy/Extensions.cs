using Albatross.Config;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Sample.Proxy {
	public static partial class Extensions {
		public static IServiceCollection AddSampleProjectProxy(this IServiceCollection services) {
			services.AddConfig<SampleProjectProxyConfig>();
			services.AddHttpClient("sample-project").AddClients()
				.ConfigureHttpClient((provider, client) => {
					var config = provider.GetRequiredService<SampleProjectProxyConfig>();
					client.BaseAddress = new Uri(config.EndPoint);
				});
			return services;
		}
	}
}