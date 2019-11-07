using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Xunit;

namespace Albatross.Config.UnitTest {
    public class TestAppSettings {
		[Fact]
		public void TestAppSettingBySetupConfig() {
			ServiceCollection svc = new ServiceCollection();
			svc.AddConfig<MySetting, GetMySetting>();
			new SetupConfig().RegisterServices(svc);
			using (var provider = svc.BuildServiceProvider()) {
				MySetting my = provider.GetRequiredService<MySetting>();
				Assert.Equal(100, my.Data.Count);
			}
		}

		[Fact]
		public void TestEnvironmentOverrideAppSettingBySetupConfig() {
			int value = 12345;
			const string prefix = "TEST_";
			System.Environment.SetEnvironmentVariable(prefix + "my__Data__Count", value.ToString());
			ServiceCollection svc = new ServiceCollection();
			svc.AddConfig<MySetting, GetMySetting>();
			new SetupConfig(null, prefix).RegisterServices(svc);
			using (var provider = svc.BuildServiceProvider()) {
				MySetting my = provider.GetRequiredService<MySetting>();
				IConfiguration cfg = provider.GetRequiredService<IConfiguration>();
				Assert.Equal(value, my.Data.Count);
			}
		}
    }
}