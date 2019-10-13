using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;

namespace Albatross.Config.UnitTest {
	public class Startup {
		public void ConfigureServices(IServiceCollection services) {
			services.AddConfig<MySetting, GetMySetting>();
		}
		public void Configure() { }
	}

	[TestFixture]
	public class TestHostSettings {

		[Test]
		public void TestHostSettingByWebHostBuilder() {
			const string environment = nameof(TestHostSettingByWebHostBuilder);

			System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
			string directory = Directory.GetCurrentDirectory();
			if (!directory.EndsWith(Path.DirectorySeparatorChar)) { directory = directory + Path.DirectorySeparatorChar; }

			var host = new WebHostBuilder()
				.UseStartup<Startup>().Build();

			IHostingEnvironment env = host.Services.GetService<IHostingEnvironment>();
			Assert.NotNull(env);
			Assert.AreEqual(directory, env.ContentRootPath);
			Assert.AreEqual(environment, env.EnvironmentName);
			// ApplicationName is the entry assembly name
			Assert.AreEqual(this.GetType().Assembly.GetName().Name, env.ApplicationName);
		}


		[Test]
		public void TestHostSettingWithANonDefaultPrefix() {
			const string environment = nameof(TestHostSettingByWebHostBuilder);
			const string prefix = "xxx_";

			System.Environment.SetEnvironmentVariable(prefix + "ENVIRONMENT", environment);
			var setup = new SetupConfig(null, prefix);
			string directory = Directory.GetCurrentDirectory();
			if (!directory.EndsWith(Path.DirectorySeparatorChar)) { directory = directory + Path.DirectorySeparatorChar; }

			var host = new WebHostBuilder()
				.UseConfiguration(setup.Configuration)
				.UseStartup<Startup>().Build();

			IHostingEnvironment env = host.Services.GetService<IHostingEnvironment>();
			Assert.NotNull(env);
			Assert.AreEqual(directory, env.ContentRootPath);
			Assert.AreEqual(environment, env.EnvironmentName);
			// ApplicationName is the entry assembly name
			Assert.AreEqual(this.GetType().Assembly.GetName().Name, env.ApplicationName);
		}
	}
}
