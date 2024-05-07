using Albatross.Config;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace Albatross.SpecFlowPlugin {
	public abstract class SpecFlowHost : IDisposable{
		public string Environment { get; }
		protected IConfiguration configuration;
		protected IHost host;
		public IServiceProvider RootServiceProvider => this.host.Services;



		public SpecFlowHost(Assembly testAssembly) {
			var basePath = new FileInfo(testAssembly.Location).DirectoryName
				?? throw new InvalidOperationException($"Test Assembly {testAssembly.FullName} doesnot have a location");
			this.Environment = EnvironmentSetting.ASPNETCORE_ENVIRONMENT.Value;

			var setupSerilog = new SetupSerilog().UseConfigFile(Environment, basePath, null);
			var logger = setupSerilog.Create();
			logger.Information($"Creating SpecFlowHost {testAssembly.FullName} for {Environment}");

			var hostBuilder = Host.CreateDefaultBuilder().UseSerilog();
			this.configuration = new ConfigurationBuilder()
				.SetBasePath(basePath!)
				.AddJsonFile("appsettings.json", false, false)
				.AddJsonFile($"appsettings.{Environment}.json", true, false)
				.AddEnvironmentVariables()
				.Build();

			this.host = hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(configuration);
			}).ConfigureServices((context, services) => this.ConfigureServices(services, context.Configuration))
			.Build();
			Init();
		}

		public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
			services.TryAddSingleton<EnvironmentSetting>(EnvironmentSetting.ASPNETCORE_ENVIRONMENT);
			services.TryAddSingleton<Microsoft.Extensions.Logging.ILogger>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("default"));
		}
		protected abstract void Init();
		public void Dispose() {
			host.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
