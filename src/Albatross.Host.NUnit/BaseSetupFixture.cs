using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Serilog;
using System;

namespace Albatross.Host.NUnit {
	public class BaseSetupFixture {
		protected IHost host;

		[OneTimeSetUp]
		public void Setup() {
			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger();

			host = Microsoft.Extensions.Hosting.Host
				.CreateDefaultBuilder()
				.UseSerilog()
				.ConfigureServices(svc=>RegisterServices(svc))
				.Build();
		}

		public IServiceProvider Provider => this.host.Services;
		public virtual void RegisterServices(IServiceCollection services) {
			services.AddTransient(provider => provider.CreateScope());
		}

		[OneTimeTearDown]
		public void TearDown() {
			host.Dispose();			
			Log.CloseAndFlush();
		}
	}
}
