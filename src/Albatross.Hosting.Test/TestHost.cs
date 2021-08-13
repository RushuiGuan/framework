using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Albatross.Logging;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Logging;
using Albatross.Config.Core;

namespace Albatross.Hosting.Test {
	public class TestHost : IDisposable {
		public const string Environment = "test";
		protected IHost host;
		public IServiceProvider Provider => this.host.Services;

		static TestHost(){
			new SetupSerilog().UseConsole(Serilog.Events.LogEventLevel.Debug).UseConfigFile("serilog.json");
		}

		public TestHost() {
			string folder = new FileInfo(this.GetType().Assembly.Location).DirectoryName;
			var hostBuilder = Host.CreateDefaultBuilder().UseSerilog();
			var configuration = new ConfigurationBuilder()
				.SetBasePath(folder)
				.AddJsonFile("appsettings.json", false, false)
				.AddJsonFile($"appsettings.{Environment}.json", true, false)
				.AddEnvironmentVariables()
				.Build();

			host = hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(configuration);
			}).ConfigureServices((ctx, svc) => RegisterServices(ctx.Configuration, svc))
			.Build();

			var logger = host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();
			InitAsync(host.Services.GetRequiredService<IConfiguration>(), logger).Wait();
		}

		public virtual Task InitAsync(IConfiguration configuration, Microsoft.Extensions.Logging.ILogger logger) {
			return Task.CompletedTask;
		}

		public virtual void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			services.AddSingleton(new EnvironmentSetting(Environment));
			services.AddSingleton<Microsoft.Extensions.Logging.ILogger>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("default"));
			services.AddTransient(provider => provider.CreateScope());
			services.AddTransient<TestScope>();
		}

		public TestScope Create() {
			return Provider.GetRequiredService<TestScope>();
		}

		public void Dispose() {
			host.Dispose();
		}
	}
}