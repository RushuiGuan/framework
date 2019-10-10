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
			System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "dev");
			string directory = Directory.GetCurrentDirectory();
			if (!directory.EndsWith(Path.DirectorySeparatorChar)) {
				directory = directory + Path.DirectorySeparatorChar;
			}

			var setup = new SetupConfig(directory);
			var host = new WebHostBuilder()
				.UseConfiguration(setup.Configuration)
				.UseStartup<Startup>().Build();

			IHostingEnvironment env = host.Services.GetService<IHostingEnvironment>();
			Assert.NotNull(env);
			Assert.AreEqual(directory, env.ContentRootPath);
			Assert.AreEqual("dev", env.EnvironmentName);
			// ApplicationName is the entry assembly name
			Assert.AreEqual(this.GetType().Assembly.GetName().Name, env.ApplicationName);
			// Assert.AreEqual("http://localhost:2000", env.WebRootPath);
		}

		[Test]
		public void EnvironmentalOverride() {

			System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "test");
			string directory = Directory.GetCurrentDirectory();
			if (!directory.EndsWith(Path.DirectorySeparatorChar)) {
				directory = directory + Path.DirectorySeparatorChar;
			}
			var setup = new SetupConfig(directory);
			var host = new WebHostBuilder()
				.UseConfiguration(setup.Configuration)
				.UseStartup<Startup>().Build();


			IHostingEnvironment env = host.Services.GetService<IHostingEnvironment>();
			Assert.NotNull(env);
			Assert.AreEqual("test", env.EnvironmentName);

		}
	}
}
