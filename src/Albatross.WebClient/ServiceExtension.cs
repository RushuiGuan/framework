using Albatross.Config;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

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

		/*
		public static async Task<string> UseClientCredential(this HttpClient client, ClientAuthorizationSetting clientAuthorizaitonSetting, string scope) {
			var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest {
				Address = clientAuthorizaitonSetting.TokenEndPoint,
				ClientId = clientAuthorizaitonSetting.ClientID,
				ClientSecret = clientAuthorizaitonSetting.ClientSecret,
				Scope = scope,
			});

            if (tokenResponse.IsError) { throw new Albatross.WebClient.ClientException(tokenResponse.Error); }
			client.SetBearerToken(tokenResponse.AccessToken);
            return tokenResponse.AccessToken;
		}

		public static async Task<string> UseResourceOwnerPassword(this HttpClient client, string username, string password, ClientAuthorizationSetting clientAuthorizaitonSetting, string scope) {
			var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest {
				Address = clientAuthorizaitonSetting.TokenEndPoint,
				ClientId = clientAuthorizaitonSetting.ClientID,
				ClientSecret = clientAuthorizaitonSetting.ClientSecret,
				UserName = username,
				Password = password,
				Scope = scope,
			});
			if (tokenResponse.IsError) { throw new Exception(tokenResponse.Error); }
			client.SetBearerToken(tokenResponse.AccessToken);
			return tokenResponse.AccessToken;
		}
		*/
	}
}
