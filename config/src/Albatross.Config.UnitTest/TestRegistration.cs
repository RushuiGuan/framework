using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Albatross.Config.UnitTest {
    [Category(nameof(Albatross.Config))]
    public class TestRegistration {
        [Test]
        public void Run() {
            ServiceCollection svc = new ServiceCollection();
            svc.AddCustomConfig(this.GetType().Assembly, true);
            ServiceProvider provider = svc.BuildServiceProvider();

            Assert.NotNull(provider.GetService<ProgramSetting>());
            Assert.NotNull(provider.GetService<IGetAssemblyLocation>());
            Assert.NotNull(provider.GetService<IGetConfigValue>());
			Assert.NotNull(provider.GetService<GetDbConfigValue>());
		}
    }
}