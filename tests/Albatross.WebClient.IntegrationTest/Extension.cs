﻿using Microsoft.Extensions.DependencyInjection;
using Albatross.Config;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Albatross.WebClient.IntegrationTest {
	public static class Extension {
		public static IServiceCollection AddTestClientService(this IServiceCollection services) {
			services.AddHttpClient<SecuredProxyService>()
			   .AddTypedClient<ValueProxyService>().ConfigureHttpClient((provider, client) => {
				   client.Timeout = TimeSpan.FromSeconds(2);
				   client.BaseAddress = new Uri(provider.GetRequiredService<MyConfig>().TestUrl);
			   });

			services.AddConfig<MyConfig, GetMyConfig>();
			return services;
		}
	}
}
