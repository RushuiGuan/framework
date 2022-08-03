using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Albatross.WebClient.Test {
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

		[Fact]
		public void TestUrl() {
			Uri baseUri = new Uri("http://localhost");
			string relativeUrl = "/test";
			var newUri = new Uri(baseUri, relativeUrl);
			Assert.Equal("http://localhost/test", newUri.ToString());
		}

		[Fact]
		public async Task TestPost() {
			var scope = host.Create();
			using var writer = new StreamWriter(@"c:\temp\test.json");
			var proxy = scope.Get<ValueProxyService>();
			proxy.UseTextWriter(writer);
			var result = await proxy.Post(new PayLoad { Number = 100 });
			Assert.NotNull(result);
		}
	}
}
