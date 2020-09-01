using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

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

		/// <summary>
		/// return named string specified in the endpoints section of the json configuration
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="name">endpoint name</param>
		/// <returns></returns>
		public static string GetEndPoint(this IConfiguration configuration, string name) => configuration.GetSection($"endpoints:{name}").Value;
	}
}
