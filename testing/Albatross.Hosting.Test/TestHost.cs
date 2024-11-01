using Albatross.Config;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace Albatross.DependencyInjection.Testing {
	public class TestHost : IDisposable {
		public const string Environment = "test";
		protected IHost host;
		public IServiceProvider Provider => this.host.Services;

		public TestHost() {
			var hostBuilder = Host.CreateDefaultBuilder().UseSerilog();
			var configuration = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", true, false)
				.AddJsonFile($"appsettings.{Environment}.json", true, false)
				.AddEnvironmentVariables()
				.Build();

			host = hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(configuration);
			}).ConfigureServices((ctx, svc) => RegisterServices(ctx.Configuration, svc))
			.Build();
		}


		public virtual void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			services.AddSingleton(new EnvironmentSetting(Environment));
		}

		public IServiceScope Create() {
			return Provider.CreateAsyncScope();
		}

		public void Dispose() {
			host.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}