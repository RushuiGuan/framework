using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Albatross.Config.UnitTest {
    [Category(nameof(Albatross.Config))]
    public class TestRegistration {
        [Test]
        public void RunSetupConfig() {
            ServiceCollection svc = new ServiceCollection();
			new SetupConfig(this.GetType().Assembly).RegisterServices(svc);
			ServiceProvider provider = svc.BuildServiceProvider();

            Assert.NotNull(provider.GetService<ProgramSetting>());
            Assert.NotNull(provider.GetService<IGetAssemblyLocation>());
		}

		[Test]
		public void RunSecondaryRegistration() {
			ServiceCollection svc = new ServiceCollection();
			svc.AddConfig(this.GetType().Assembly, new ConfigurationBuilder().Build());
			ServiceProvider provider = svc.BuildServiceProvider();

			Assert.NotNull(provider.GetService<ProgramSetting>());
			Assert.NotNull(provider.GetService<IGetAssemblyLocation>());
		}
	}
}