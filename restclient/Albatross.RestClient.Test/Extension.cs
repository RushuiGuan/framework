using Albatross.Config;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.RestClient.Test {
	public static class Extension {
		public static IServiceCollection AddTestClientService(this IServiceCollection services) {
			services.AddHttpClient("test")
			   .ConfigureHttpClient((provider, client) => {
				   client.Timeout = TimeSpan.FromSeconds(2);
				   client.BaseAddress = new Uri(provider.GetRequiredService<MyConfig>().TestUrl);
			   });
			services.AddConfig<MyConfig>();
			return services;
		}
	}
}