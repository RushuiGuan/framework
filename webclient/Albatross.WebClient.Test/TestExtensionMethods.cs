using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Albatross.WebClient.Test {
	public partial class TestWebClient  {
		[Fact]
		public void TestConfig() {
			using var host = My.Create();
			var config = host.Services.GetRequiredService<MyConfig>();
			Assert.True(config.TestUrl.EndsWith('/'));
			Assert.True(config.Test1Url.EndsWith('/'));
		}
	}
}