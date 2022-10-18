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
			Assert.True(config.TestUrl.EndsWith('/'));
			Assert.True(config.Test1Url.EndsWith('/'));
		}
	}
}
