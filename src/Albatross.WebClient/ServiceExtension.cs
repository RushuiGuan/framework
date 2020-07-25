using Albatross.Config;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Albatross.WebClient {
	public static class ServiceExtension {
		public static IServiceCollection AddClientAuthorizationSetting(this IServiceCollection services) {
			services.AddConfig<ClientAuthorizationSetting, GetClientAuthorizationSetting>();
			return services;
		}
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

		public static async Task<string> UseClientCredential(this HttpClient client, ClientAuthorizationSetting setting) {
			var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest {
				Address = setting.TokenEndPoint,
				ClientId = setting.ClientID,
				ClientSecret = setting.ClientSecret,
				Scope = string.Join(" ", setting.Scopes),
			});

            if (tokenResponse.IsError) { throw new Albatross.WebClient.ClientException(tokenResponse.Error); }
			client.SetBearerToken(tokenResponse.AccessToken);
            return tokenResponse.AccessToken;
		}

		public static async Task<string> UseResourceOwnerPassword(this HttpClient client, string username, string password, ClientAuthorizationSetting setting) {
			var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest {
				Address = setting.TokenEndPoint,
				ClientId = setting.ClientID,
				ClientSecret = setting.ClientSecret,
				UserName = username,
				Password = password,
				Scope = string.Join(" ", setting.Scopes),
			});
			if (tokenResponse.IsError) { throw new Exception(tokenResponse.Error); }
			client.SetBearerToken(tokenResponse.AccessToken);
			return tokenResponse.AccessToken;
		}
	}
}
