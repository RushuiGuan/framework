using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Albatross.Config.UnitTest {
	public class TestFakeConfiguration : IClassFixture<MyTestHost> {
		IConfigurationSection? Get(IConfiguration config, string path) {
			var keys = path.Split(':');
			IConfigurationSection? section = null;
			for (int i = 0; i < keys.Length; i++) {
				if (i == 0) {
					section = config.GetSection(keys[i]);
				} else {
					section = section?.GetSection(keys[i]);
				}
			}
			return section;
		}

		[Theory]
		[InlineData("", null)]
		[InlineData("a:b", "any")]
		[InlineData("ConnectionStrings:a", "any")]
		public void TestAnyConfiguration(string path, string expectedValue) {
			var config = new AnyConfiguration();
			var section = Get(config, path);
			Assert.Equal(expectedValue, section?.Value);
		}

		[Fact]
		public void TestAnyConfigurationCalls() {
			var config = new AnyConfiguration();
			Assert.Equal(AnyConfiguration.Any, config.GetRequiredConnectionString("a"));
			Assert.Equal(AnyConfiguration.Any, config.GetConnectionString("a"));
			Assert.Equal(AnyConfiguration.Any + "/", config.GetRequiredEndPoint("a"));
			Assert.Equal(AnyConfiguration.Any + "/", config.GetEndPoint("a"));

			config["ConnectionStrings:b"] = "test";
			Assert.Equal("test", config.GetConnectionString("b"));
			Assert.Equal("test", config.GetRequiredConnectionString("b"));

			config["endpoints:c"] = "test";
			Assert.Equal("test", config.GetEndPoint("c", false));
			Assert.Equal("test", config.GetRequiredEndPoint("c", false));
		}

		[Theory]
		[InlineData("ConnectionStrings:a", "aaa", "ConnectionStrings:b", "any")]
		[InlineData("ConnectionStrings:a", "aaa", "ConnectionStrings:a", "aaa")]
		public void TestSettingAnyConfiguration(string settingPath, string settingValue,  string gettingPath, string expectedValue) {
			var config = new AnyConfiguration();
			config[settingPath] = settingValue;
			Assert.Equal(expectedValue, config[gettingPath]);
		}
	}
}
