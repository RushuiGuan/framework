using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.WebClient;
using System.Collections.Generic;
using Xunit;
using System.Collections.Specialized;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.WebClient.IntegrationTest {
	public partial class TestWebClient : IClassFixture<MyTestHost>{
		private readonly MyTestHost host;

		public TestWebClient(MyTestHost host) {
			this.host = host;
		}

		[Fact]
		public void TestConfig() {
			var config = host.Provider.GetRequiredService<MyConfig>();
			Assert.False(config.TestUrl.EndsWith('/'));
		}

		[Fact]
		public async Task TestTimeOut() {
			var scope = host.Create();
			var proxy = scope.Get<ValueProxyService>();
			try {
				await proxy.Timeout(10);
			}catch(TaskCanceledException err) {
				Assert.True(err.InnerException is TimeoutException);
			}
		}
	}
}
