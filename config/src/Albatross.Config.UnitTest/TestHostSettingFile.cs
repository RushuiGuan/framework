using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;

namespace Albatross.Config.UnitTest {
	public class Startup {
		public void Configure() { }
 }
	[TestFixture]
	public class TestHostSettingFile {

		[Test]
		public void BasicCheck() {
			string contentRoot = @"C:\app\framework\config\src\Albatross.Config.UnitTest";
			var setup = new SetupConfig(contentRoot, useHostSetting: true);
			var host = WebHost.CreateDefaultBuilder()
				.UseConfiguration(setup.Configuration)
				.UseStartup<Startup>().Build();

			IHostingEnvironment env = host.Services.GetService<IHostingEnvironment>();
			Assert.NotNull(env);
			Assert.AreEqual(contentRoot, env.ContentRootPath);
		}
	}
}
