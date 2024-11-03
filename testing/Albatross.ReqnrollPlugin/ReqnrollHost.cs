using Albatross.Config;
using Albatross.Logging;
using Reqnroll.BoDi;
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
using Reqnroll;
using Reqnroll.Infrastructure;

namespace Albatross.ReqnrollPlugin {
	public abstract class ReqnrollHost : IDisposable {
		public string Environment { get; }
		protected IConfiguration configuration;
		protected IHost host;
		protected Logger logger;
		protected Assembly testAssembly;
		private readonly IObjectContainer rootBodiContainer;
		private ConcurrentDictionary<ReqnrollContext, IServiceScope> activeServiceScopes = new ConcurrentDictionary<ReqnrollContext, IServiceScope>();
		private ConcurrentDictionary<IServiceProvider, IContextManager> contextManagers = new ConcurrentDictionary<IServiceProvider, IContextManager>();

		public ReqnrollHost(Assembly testAssembly, IObjectContainer rootBodiContainer) {
			Albatross.Logging.Extensions.RemoveLegacySlackSinkOptions();
			this.testAssembly = testAssembly;
			this.rootBodiContainer = rootBodiContainer;
			var basePath = new FileInfo(testAssembly.Location).DirectoryName
				?? throw new InvalidOperationException($"Test Assembly {testAssembly.FullName} doesnot have a location");
			this.Environment = EnvironmentSetting.DOTNET_ENVIRONMENT.Value;

			var setupSerilog = new SetupSerilog().Configure(cfg => ConfigureSerilog(cfg, Environment, basePath));
			this.logger = setupSerilog.Create();
			logger.Information($"Creating ReqnrollHost {testAssembly.FullName} with env = {Environment}");

			var hostBuilder = Host.CreateDefaultBuilder().UseSerilog();
			this.configuration = new ConfigurationBuilder()
				.SetBasePath(basePath!)
				.AddJsonFile("appsettings.json", true, false)
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

		protected virtual void ConfigureSerilog(LoggerConfiguration cfg, string environment, string basePath) {
			SetupSerilog.UseConsole(cfg, Serilog.Events.LogEventLevel.Information);
		}

		public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
			services.TryAddSingleton(EnvironmentSetting.DOTNET_ENVIRONMENT);
			services.TryAddSingleton(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("default"));

			services.TryAddSingleton(this.rootBodiContainer);
			services.TryAddTransient(provider => this.contextManagers[provider]);
			services.TryAddTransient(provider => this.contextManagers[provider].TestThreadContext);
			services.TryAddTransient(provider => this.contextManagers[provider].FeatureContext);
			services.TryAddTransient(provider => this.contextManagers[provider].ScenarioContext);
			foreach (var type in this.testAssembly.GetTypes()) {
				if (type.GetCustomAttribute<BindingAttribute>() != null
					&& !type.IsAbstract
					&& !type.IsInterface
					&& type.IsClass
					&& !type.IsGenericTypeDefinition) {
					services.TryAddScoped(type);
				}
			}
		}
		protected virtual void Init() {
			logger.Information("Initializing ReqnrollHost");
		}
		public void Dispose() {
			logger.Information("Disposing ReqnrollHost");
			host.Dispose();
			GC.SuppressFinalize(this);
		}

		public IServiceScope CreateScope(IContextManager contextManager, FeatureContext feature, ScenarioContext scenario) {
			logger.Information("Creating scope: feature={feature}, scenario={scenario}", feature.FeatureInfo.Title, scenario.ScenarioInfo.Title);
			var scope = host.Services.CreateScope();
			contextManagers.TryAdd(scope.ServiceProvider, contextManager);
			return activeServiceScopes.GetOrAdd(scenario, scope);
		}
		public void DisposeScope(ScenarioContext context) {
			if (activeServiceScopes.TryRemove(context, out var scope)) {
				this.contextManagers.TryRemove(scope.ServiceProvider, out _);
				logger.Information("Disposing scope: scenario={name}", context.ScenarioInfo.Title);
				scope.Dispose();
			}
		}
	}
}