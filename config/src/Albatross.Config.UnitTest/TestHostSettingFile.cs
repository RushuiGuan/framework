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
			string directory = Directory.GetCurrentDirectory();
			if (!directory.EndsWith(Path.DirectorySeparatorChar)) {
				directory = directory + Path.DirectorySeparatorChar;
			}
			var setup = new SetupConfig(directory);
			var host = new WebHostBuilder()
				.UseConfiguration(setup.Configuration)
				.UseStartup<Startup>().Build();


			IWebHostEnvironment env = host.Services.GetService<IWebHostEnvironment>();
			Assert.NotNull(env);
			Assert.AreEqual(directory, env.ContentRootPath);
			Assert.AreEqual("dev", env.EnvironmentName);
			Assert.AreEqual("http://localhost:2000", env.WebRootPath);
			Assert.AreEqual("test-app", env.ApplicationName);
		
		}
	}
}
