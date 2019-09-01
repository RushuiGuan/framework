using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Albatross.WebClient.IntegrationTest {
	public static class Extension {
		public static IHttpClientBuilder WithTestClientService(this IServiceCollection services, Action<HttpClient> configure) {
			return services.AddHttpClient<ValueClientService>(configure);
		}
	}
}
