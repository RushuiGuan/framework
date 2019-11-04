using Albatross.Config;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.UnitTest {
	[SetUpFixture]
	public class Setup {
		public static IServiceProvider ServiceProvider { get; private set; }
		[OneTimeSetUp]
		public static void Run() {
			var setup = new SetupConfig(Extension.GetAssemblyLocation(typeof(Setup)));
			setup.UseSerilog();
			ServiceCollection services = new ServiceCollection();
			setup.RegisterServices(services);
			ServiceProvider = services.BuildServiceProvider();
			new Migrate(setup.Configuration).Run();
		}
	}
}
