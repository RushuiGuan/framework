using Albatross.Config;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using TechTalk.SpecFlow;

namespace Albatross.SpecFlowPlugin {
	public abstract class SpecFlowHost : IDisposable{
		public string Environment { get; }
		protected IConfiguration configuration;
		protected IHost host;
		protected Logger logger;
		protected Assembly testAssembly;
		private ConcurrentDictionary<SpecFlowContext, IServiceScope> activeServiceScopes = new ConcurrentDictionary<SpecFlowContext, IServiceScope>();	
		
		public SpecFlowHost(Assembly testAssembly) {
			Albatross.Logging.Extensions.RemoveLegacySlackSinkOptions();
			this.testAssembly = testAssembly;
			var basePath = new FileInfo(testAssembly.Location).DirectoryName
				?? throw new InvalidOperationException($"Test Assembly {testAssembly.FullName} doesnot have a location");
			this.Environment = EnvironmentSetting.ASPNETCORE_ENVIRONMENT.Value;

			var setupSerilog = new SetupSerilog().UseConfigFile(Environment, basePath, null);
			this.logger = setupSerilog.Create();
			logger.Information($"Creating SpecFlowHost {testAssembly.FullName} with env = {Environment}");

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
			foreach(var type in this.testAssembly.GetTypes()) {
				if(type.GetCustomAttribute<BindingAttribute>() != null 
					&&!type.IsAbstract 
					&& !type.IsInterface 
					&& type.IsClass 
					&& !type.IsGenericTypeDefinition) { 
					services.TryAddScoped(type);
				}
			}
		}
		protected virtual void Init() {
			logger.Information("Initializing SpecFlowHost");
		}
		public void Dispose() {
			logger.Information("Disposing SpecFlowHost");
			host.Dispose();
			GC.SuppressFinalize(this);
		}

		public IServiceScope CreateScope(FeatureContext context) {
			logger.Information("Creating feature scope for {name}", context.FeatureInfo.Title);
			return activeServiceScopes.GetOrAdd(context, this.host.Services.CreateScope());
		}
		public void DisposeScope(FeatureContext context) {
			if (activeServiceScopes.TryRemove(context, out var scope)) {
				logger.Information("Disposing feature scope for {name}", context.FeatureInfo.Title);
				scope.Dispose();
			}
		}
	}
}
