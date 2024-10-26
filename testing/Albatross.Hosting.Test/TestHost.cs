using Albatross.Config;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Hosting.Test {
	public class TestHost : IDisposable {
		public const string Environment = "test";
		protected IHost host;
		public IServiceProvider Provider => this.host.Services;

		static TestHost() {
			new SetupSerilog().UseConsole(Serilog.Events.LogEventLevel.Debug).UseConfigFile("serilog.json", null, null);
		}

		public TestHost() {
			var folder = new FileInfo(this.GetType().Assembly.Location).DirectoryName;
			var hostBuilder = Host.CreateDefaultBuilder().UseSerilog();
			var configuration = new ConfigurationBuilder()
				.SetBasePath(folder!)
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