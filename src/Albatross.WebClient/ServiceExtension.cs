using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Albatross.WebClient {
	public static class ServiceExtension {
		public static IHttpClientBuilder SetBaseUrl(this IHttpClientBuilder builder, Func<Uri> getBaseUrl) {
			builder.ConfigureHttpClient(client => {
				client.BaseAddress = getBaseUrl();
			});
			return builder;
		}

		public static IHttpClientBuilder UseWindowsAuthentication(this IHttpClientBuilder builder) {
			builder.ConfigurePrimaryHttpMessageHandler(() => {
				return new HttpClientHandler() {
					AllowAutoRedirect = false,
					UseDefaultCredentials = true,
				};
			});
			return builder;
		}
	}
}
