using Albatross.Config.Core;
using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Albatross.Config.UnitTest {
	public class MyTestHost : TestHost {
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddTransient<GetGoogleUrl>();
			services.AddTransient<GetNoLongerRequiredConfig>();
			services.AddConfig<ProgramSetting, GetProgramSetting>();
			services.AddConfig<DbConfig, GetDbConfig>();
			services.AddConfig<EmptyConfig, GetEmptyConfig>();
		}
	}

	public class Tests : IClassFixture<MyTestHost> {
		private readonly MyTestHost host;

		public Tests(MyTestHost host) {
			this.host = host;
		}

		[Fact]
		public void TestGetProgramSetting() {
			var setting = host.Provider.GetRequiredService<ProgramSetting>();
			Assert.NotNull(setting);
			Assert.NotEmpty(setting.App);
			Assert.NotEmpty(setting.Group);
		}

		[Fact]
		public void TestGetGoogleUrl() {
			var value = host.Provider.GetRequiredService<GetGoogleUrl>().Get();
			Assert.NotNull(value);
		}

		[Fact]
		public void TestGetNoLongerRequiredConfig() {
			var handle = host.Provider.GetRequiredService<GetNoLongerRequiredConfig>();
			var config = handle.Get();
			Assert.NotNull(config);
		}

		[Fact]
		public void TestDbConfig() {
			var handle = host.Provider.GetRequiredService<GetDbConfig>();
			var cfg = handle.Get();
			Assert.NotNull(cfg.DbConnection);
		}

		[Fact]
		public void TestEmptyConfig() {
			var handle = host.Provider.GetRequiredService<GetEmptyConfig>();
			var cfg = handle.Get();
			Assert.NotNull(cfg);
		}
	}
}