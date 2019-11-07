using Albatross.Config.Core;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Albatross.Config.UnitTest {
    public class TestRegistration {
        [Fact]
        public void RunSetupConfig() {
            ServiceCollection svc = new ServiceCollection();
			new SetupConfig(this.GetType().GetAssemblyLocation()).RegisterServices(svc);
			ServiceProvider provider = svc.BuildServiceProvider();
			Assert.NotNull(provider.GetService<ProgramSetting>());
		}

		[Fact]
		public void RunSecondaryRegistration() {
			ServiceCollection svc = new ServiceCollection();
			var setup = new SetupConfig(this.GetType().GetAssemblyLocation());
			setup.RegisterServices(svc);
			ServiceProvider provider = svc.BuildServiceProvider();
			Assert.NotNull(provider.GetService<ProgramSetting>());
		}
	}
}