using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;

namespace Albatross.Config.UnitTest {
	[Category(nameof(Albatross.Config))]
    public class TestAppSettings {
		[Test]
		public void TestNormal() {
			ServiceCollection svc = new ServiceCollection();
			new SetupConfig(Directory.GetCurrentDirectory()).RegisterServices(svc);
			svc.AddConfig<MySetting, GetMySetting>();
			using (var provider = svc.BuildServiceProvider()) {
				MySetting my = provider.GetRequiredService<MySetting>();
				Assert.AreEqual(100, my.Data.Count);
			}
		}

		[Test]
		public void TestOverride() {
			ServiceCollection svc = new ServiceCollection();
			System.Environment.SetEnvironmentVariable("my__data__count", "200");
			new SetupConfig(Directory.GetCurrentDirectory(), "test_").RegisterServices(svc);
			svc.AddConfig<MySetting, GetMySetting>();
			using (var provider = svc.BuildServiceProvider()) {
				MySetting my = provider.GetRequiredService<MySetting>();
				Assert.AreEqual(200, my.Data.Count);
			}
		}
    }
}