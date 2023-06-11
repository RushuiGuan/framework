using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Albatross.Config.UnitTest {

	public class Tests : IClassFixture<MyTestHost> {
		private readonly MyTestHost host;

		public Tests(MyTestHost host) {
			this.host = host;
		}

		[Fact]
		public void TestGetProgramSetting() {
			var setting = host.Provider.GetRequiredService<ProgramSetting>();
			Assert.NotNull(setting);
			Assert.Equal("config-unittest", setting.App);
			Assert.Equal("config", setting.Group);
			Assert.Equal("windows", setting.ServiceManager);
		}

		/// <summary>
		/// The config value is just a string in this case.  The Config class will have to override the base class
		/// to get this value
		/// </summary>
		[Fact]
		public void TestSingleValueConfig() {
			var value = host.Provider.GetRequiredService<SingleValueConfig>();
			Assert.NotNull(value);
			Assert.Equal("www.google.com", value.Value);
		}

		/// <summary>
		/// The config value comes from a json object
		/// </summary>
		[Fact]
		public void TestSerializedConfig() {
			var config = host.Provider.GetRequiredService<MySetting>();
			Assert.NotNull(config);
			Assert.Equal("my test data", config.Name);
			Assert.NotNull(config.Data);
			Assert.Equal(100, config.Data?.Count);
		}

		/// <summary>
		/// Config with no keys can get its value from other (shared) parts of configuration, such as connectionStrings and endpoints
		/// </summary>
		[Fact]
		public void TestConfigWithNoKey() {
			var cfg = host.Provider.GetRequiredService<ConfigWithNoKey>();
			Assert.NotNull(cfg);
			Assert.Equal("azure-db", cfg.ConnectionString);
			Assert.Equal("microsoft.com/", cfg.EndPoint);
		}

		/// <summary>
		/// The config is completely missing in the config file and it should be allowed.  Validation will kick in
		/// if any of the data is required
		/// </summary>
		[Fact]
		public void TestNotDefinedConfig() {
			var cfg = host.Provider.GetRequiredService<DbConfig>();
			Assert.NotNull(cfg);
			Assert.Null(cfg.Data);
		}

		[Fact]
		public void TestValidation() {
			Assert.Throws<ValidationException>(() => host.Provider.GetRequiredService<ValidationTest>());
		}
	}
}