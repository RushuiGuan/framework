using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Albatross.Config.UnitTest {
	public class TestRealConfiguration {

		[Theory]
		[InlineData("")]
		[InlineData("program")]
		[InlineData("program:app")]
		[InlineData("single-value-config")]
		[InlineData("ConnectionStrings:configDatabaseConnection")]
		public void TestGetPath(string path) {
			using var host = My.Create();
			using var scope = host.Services.CreateScope();
			var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
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
			var host = My.Create();
			using var scope = host.Services.CreateScope();
			var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
			var section = config.GetSection(path);
			Assert.Equal(expectedValue, section?.Value);
		}

		[Theory]
		[InlineData("", null)]
		[InlineData("program", null)]
		[InlineData("program:app", "config-unittest")]
		[InlineData("ConnectionStrings:my-database", "azure-db")]
		public void TestGetValueByIndex(string path, string? expectedValue) {
			using var host = My.Create();
			using var scope = host.Services.CreateScope();
			var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
			var value = config[path];
			Assert.Equal(expectedValue, value);
		}

		[Fact]
		public void VerifyNotNullCheck() {
			using var host = My.Create();
			using var scope = host.Services.CreateScope();
			var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
			var section = config.GetSection(string.Empty);
			Assert.NotNull(section);
		}
	}
}