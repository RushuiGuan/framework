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
			services.AddTransient<GetRequiredConfig>();
			services.AddConfig<ProgramSetting, GetProgramSetting>();
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
			Assert.NotEmpty(setting.Environment);
		}

		[Fact]
		public void TestGetGoogleUrl() {
			var value = host.Provider.GetRequiredService<GetGoogleUrl>().Get();
			Assert.NotEmpty(value);
		}

		[Fact]
		public void TestGetRequiredConfig() {
			var handle = host.Provider.GetRequiredService<GetRequiredConfig>();
			Assert.Throws<ConfigurationException>(() => handle.Get());
		}
	}
}