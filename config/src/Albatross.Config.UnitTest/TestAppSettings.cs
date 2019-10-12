using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;

namespace Albatross.Config.UnitTest {
	[Category(nameof(Albatross.Config))]
    public class TestAppSettings {
		[Test]
		public void TestAppSettingBySetupConfig() {
			ServiceCollection svc = new ServiceCollection();
			svc.AddConfig<MySetting, GetMySetting>();
			new SetupConfig().RegisterServices(svc);
			using (var provider = svc.BuildServiceProvider()) {
				MySetting my = provider.GetRequiredService<MySetting>();
				Assert.AreEqual(100, my.Data.Count);
			}
		}

		[Test]
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
				Assert.AreEqual(value, my.Data.Count);
			}
		}

		[Test]
		public void TestAppSettingByWebHostBuilder() {
			var setup = new SetupConfig();
			var host = new WebHostBuilder().UseConfiguration(setup.Configuration).UseStartup<Startup>().Build();
			var my = host.Services.GetRequiredService<MySetting>();
			Assert.AreEqual(100, my.Data.Count);
		}

		[Test]
		public void TestEnvironmentOverrideAppSettingByWebHostBuilder() {
			int value = 12345;
			const string prefix = "XXX_";
			System.Environment.SetEnvironmentVariable(prefix + "my__Data__Count", value.ToString());
			var setup = new SetupConfig(null, prefix);
			var host = new WebHostBuilder().UseConfiguration(setup.Configuration).UseStartup<Startup>().Build();
			var my = host.Services.GetRequiredService<MySetting>();
			Assert.AreEqual(value, my.Data.Count);
		}
    }
}