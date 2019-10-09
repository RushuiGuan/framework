using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Albatross.Config.UnitTest {
	[Category(nameof(Albatross.Config))]
    public class TestEnvironmentalOverride {
        ServiceCollection svc = new ServiceCollection();
        ServiceProvider provider;
        [OneTimeSetUp]
        public void Setup() {
			System.Environment.SetEnvironmentVariable("test_my_data_count", "200");
			new SetupConfig(this.GetType().Assembly, "test_").RegisterServices(svc);
			svc.AddConfig<MySetting, GetMySetting>();
            provider = svc.BuildServiceProvider();
        }


		[Test]
		public void TestNormal() {
			MySetting my = provider.GetRequiredService<MySetting>();
			Assert.AreEqual(100, my.Data.Count);
		}

		[Test]
		public void TestOverride() {
			MySetting my = provider.GetRequiredService<MySetting>();
			Assert.AreEqual(200, my.Data.Count);
		}
    }
}