using Albatross.Config;
using Albatross.Logging;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using ExcelDna.IntelliSense;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;

namespace Albatross.DependencyInjection.Excel {
	public abstract class HostedExcelAddIn : IExcelAddIn {
		public Microsoft.Extensions.Logging.ILogger logger;
		private IServiceProvider provider => host.Services;
		private IHost host;
		private IConfiguration configuration;
		private Serilog.Core.Logger serilogLogger;
		private bool closed = false;

		protected virtual void Start(IConfiguration configuration, IServiceProvider provider) {
			RegisterRibbons(provider);
			IntelliSenseServer.Install();
		}
		protected virtual void Stop() => IntelliSenseServer.Uninstall();
		EnvironmentSetting envSetting => EnvironmentSetting.DOTNET_ENVIRONMENT;
		public string Environment => this.envSetting.Value;
		public virtual string CurrentDirectory => System.IO.Path.GetDirectoryName(typeof(HostedExcelAddIn).Assembly.Location)!;

		public HostedExcelAddIn() {
			Albatross.Logging.Extensions.RemoveLegacySlackSinkOptions();
			serilogLogger = new SetupSerilog().Configure(ConfigureLogging).Create();
			IHostBuilder hostBuilder = Host.CreateDefaultBuilder().UseSerilog();
			var configBuilder = new ConfigurationBuilder()
				.SetBasePath(CurrentDirectory)
				.AddJsonFile("appsettings.json", false, true);
			if (!string.IsNullOrEmpty(Environment)) { configBuilder.AddJsonFile($"appsettings.{Environment}.json", true, true); }
			this.configuration = configBuilder
				.AddEnvironmentVariables()
				.Build();
			host = hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(configuration);
			}).ConfigureServices((ctx, svc) => RegisterServices(ctx.Configuration, svc)).Build();
			logger = host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();
		}
		public virtual void ConfigureLogging(LoggerConfiguration cfg) {
			SetupSerilog.UseConfigFile(cfg, this.Environment, CurrentDirectory, new string[0]);
		}
		public virtual void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			services.TryAddSingleton(new ProgramSetting(configuration));
			services.TryAddSingleton(this.envSetting);
			services.AddSingleton<FunctionRegistrationService>();
			services.AddSingleton<HelpService>();
			services.AddSingleton(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger(this.GetType().FullName ?? "default"));
		}
		void RegisterRibbons(IServiceProvider provider) {
			var ribbons = provider.GetRequiredService<IEnumerable<ExcelRibbon>>();
			foreach (ExcelRibbon item in ribbons) {
				logger.LogInformation("Loading excel ribbon: {name}", item.GetType().Name);
				ExcelComAddInHelper.LoadComAddIn(item);
			}
		}
		void IExcelAddIn.AutoOpen() {
			try {
				logger.LogInformation("startup excel addin");
				Start(configuration, provider);
			} catch (Exception err) {
				logger.LogError(err, "error during startup");
			}
		}
		void IExcelAddIn.AutoClose() {
			try {
				if (!closed) {
					logger.LogInformation("stopping excel addin");
					Stop();
					logger.LogDebug("Disposing UtilityBase");
					this.host.Dispose();
					logger.LogDebug("CloseAndFlush Logging");
					serilogLogger.Dispose();
					closed = true;
				}
			} catch (Exception e) {
				logger.LogError(e, "error during shutdown");
			}
		}
	}
}