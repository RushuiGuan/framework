using Albatross.Host.NUnit;
using Autofac;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Albatross.WebClient.IntegrationTest {
	[Ignore("integration test")]
    [TestFixture]
	public class TestOAuth : TestBase<TestScope> {
        public override void RegisterPackages(IServiceCollection svc) {
            svc
                .AddHttpClient<SecuredClientService>()
                .SetBaseUrl(() => new Uri("http://localhost:20000"))
                .ConfigureHttpClient(async client => {
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

        [Test]
        public async Task TestGetDiscoveryDocument() {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:30002");
            Assert.False(disco.IsError);
        }

        [Test]
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

        [Test]
        public async Task TestAuthorizedCall() {
            using (var unitOfWork = NewUnitOfWork()) {
                var svc = unitOfWork.Get<SecuredClientService>();
                string result = await svc.GetText();
                Assert.NotNull(result);
            }
        }
    }
}
