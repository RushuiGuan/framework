using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Albatross.Config.UnitTest {
	public class TestRealConfiguration : IClassFixture<MyTestHost> {
		private readonly MyTestHost host;

		public TestRealConfiguration(MyTestHost host) {
			this.host = host;
		}

		[Theory]
		[InlineData("")]
		[InlineData("program")]
		[InlineData("program:app")]
		[InlineData("single-value-config")]
		[InlineData("ConnectionStrings:configDatabaseConnection")]
		public void TestGetPath(string path) {
			var scope = host.Create();
			var config = scope.Get<IConfiguration>();
			var section = Get(config, path);
			Assert.Equal(path, section?.Path);
		}

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
		[InlineData("program", null)]
		[InlineData("program:app", "config-unittest")]
		[InlineData("ConnectionStrings:my-database", "azure-db")]
		public void TestGetValue(string path, string? expectedValue) {
			var scope = host.Create();
			var config = scope.Get<IConfiguration>();
			var section = config.GetSection(path);
			Assert.Equal(expectedValue, section?.Value);
		}

		[Theory]
		[InlineData("", null)]
		[InlineData("program", null)]
		[InlineData("program:app", "config-unittest")]
		[InlineData("ConnectionStrings:my-database", "azure-db")]
		public void TestGetValueByIndex(string path, string? expectedValue) {
			var scope = host.Create();
			var config = scope.Get<IConfiguration>();
			var value = config[path];
			Assert.Equal(expectedValue, value);
		}

		[Fact]
		public void VerifyNotNullCheck() {
			var scope = host.Create();
			var config = scope.Get<IConfiguration>();
			var section = config.GetSection(string.Empty);
			Assert.NotNull(section);
		}
	}
}