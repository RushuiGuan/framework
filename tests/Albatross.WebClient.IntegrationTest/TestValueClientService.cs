using Albatross.Host.Test;
using Albatross.WebClient.IntegrationTest.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.WebClient.IntegrationTest {
	public class TestValueClientServiceHost : TestHost {
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.WithTestClientService(client => {
				client.BaseAddress = new Uri("http://localhost:20000");
			});
		}
	}

	public class TestValueClientService :IClassFixture<TestValueClientServiceHost> {
		private readonly TestValueClientServiceHost host;

		public TestValueClientService(TestValueClientServiceHost host) {
			this.host = host;
		}

		[Fact(Skip= "integration test")]
		public async Task TestGetText() {
			using (var scope = host.Create()) {
				var result = await scope.Get<ValueClientService>().GetText();
				Assert.NotNull(result);
                Assert.Equal(PayLoadExtension.GetText(), result);
			}
		}

		[Fact(Skip= "integration test")]
        public async Task TestGetJson() {
            using (var scope = host.Create()) {
                var result = await scope.Get<ValueClientService>().GetJson();
                Assert.NotNull(result);
                var expected = PayLoadExtension.Make();
                Assert.Equal(expected.Data, result.Data);
                Assert.Equal(expected.Date, result.Date);
                Assert.Equal(expected.DateTimeOffset, result.DateTimeOffset);
                Assert.Equal(expected.Name, result.Name);
                Assert.Equal(expected.Number, result.Number);
            }
        }

		[Fact(Skip= "integration test")]
        public async Task TestPostJson() {
            using (var scope = host.Create()) {
                var result = await scope.Get<ValueClientService>().Post(PayLoadExtension.Make());
                Assert.NotNull(result);
                var expected = PayLoadExtension.Make();
                Assert.Equal(expected.Data, result.Data);
                Assert.Equal(expected.Date, result.Date);
                Assert.Equal(expected.DateTimeOffset, result.DateTimeOffset);
                Assert.Equal(expected.Name, result.Name);
                Assert.Equal(expected.Number, result.Number);
            }
        }
    }
}
