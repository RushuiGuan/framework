using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Albatross.Config.UnitTest {
	public class Tests {


		[Fact]
		public void TestGetProgramSetting() {
			using var host = My.Create();
			var setting = host.Services.GetRequiredService<ProgramSetting>();
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
			using var host = My.Create();
			var value = host.Services.GetRequiredService<SingleValueConfig>();
			Assert.NotNull(value);
			Assert.Equal("www.google.com", value.Value);
		}

		/// <summary>
		/// The config value comes from a json object
		/// </summary>
		[Fact]
		public void TestSerializedConfig() {
			using var host = My.Create();
			var config = host.Services.GetRequiredService<MySetting>();
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
			using var host = My.Create();
			var cfg = host.Services.GetRequiredService<ConfigWithNoKey>();
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
			using var host = My.Create();
			var cfg = host.Services.GetRequiredService<DbConfig>();
			Assert.NotNull(cfg);
			Assert.Null(cfg.Data);
		}

		[Fact]
		public void TestValidation() {
			using var host = My.Create();
			Assert.Throws<ValidationException>(() => host.Services.GetRequiredService<ValidationTest>());
		}
	}
}