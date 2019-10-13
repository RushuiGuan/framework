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
			new SetupConfig(this.GetType().GetAssemblyLocation()).RegisterServices(svc);
			ServiceProvider provider = svc.BuildServiceProvider();

			Assert.NotNull(provider.GetService<ProgramSetting>());
		}

		[Test]
		public void RunSecondaryRegistration() {
			ServiceCollection svc = new ServiceCollection();
			var setup = new SetupConfig(this.GetType().GetAssemblyLocation());
			setup.RegisterServices(svc);
			ServiceProvider provider = svc.BuildServiceProvider();

			Assert.NotNull(provider.GetService<ProgramSetting>());
		}
	}
}