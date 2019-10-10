using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.UnitTest {
	[TestFixture]
	public class TestHostSettingFile {

		[Test]
		public void Run() {
			var setup = new SetupConfig(this.GetType().GetAssemblyLocation(), useHostSetting: true);
			ServiceCollection svc = new ServiceCollection();
			setup.RegisterServices(svc);
			var provider = svc.BuildServiceProvider();
			IHostingEnvironment env = provider.GetService<IHostingEnvironment>();
			Assert.NotNull(env);
		}
	}
}
