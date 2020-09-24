using Albatross.Hosting.Test;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.WebClient.IntegrationTest {
	public class TestOAuthHost : TestHost {
		public TestOAuthHost() {
		}
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services
				.AddHttpClient<SecuredClientService>()
				.ConfigureHttpClient(async client => {
					client.BaseAddress = new Uri("http://localhost:20000");
					var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest {
						Address = "http://localhost:30002/connect/token",
						ClientId = "client",
						ClientSecret = "secret",
						Scope = "test"
					});
					client.SetBearerToken(tokenResponse.AccessToken);
					client.DefaultRequestHeaders.ToString();
				});
		}
	}

	public class TestOAuth : IClassFixture<TestOAuthHost>{
		private readonly TestOAuthHost host;

		public TestOAuth(TestOAuthHost host) {
			this.host = host;
		}

        [Fact(Skip ="Integration test")]
		public async Task TestGetDiscoveryDocument() {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:30002");
            Assert.False(disco.IsError);
        }

        [Fact(Skip ="Integration test")]
		public async Task TestRequestCredentialsToken() {
            var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest {
                Address = "http://localhost:30002/connect/token",
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "test"
            });
            Assert.False(tokenResponse.IsError);
            Console.WriteLine(tokenResponse.Json);
        }

        [Fact(Skip ="Integration test")]
		public async Task TestAuthorizedCall() {
            using (var scope = host.Create()) {
                var svc = scope.Get<SecuredClientService>();
                string result = await svc.GetText();
                Assert.NotNull(result);
            }
        }
    }
}
