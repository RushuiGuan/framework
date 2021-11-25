using Albatross.Config;
using Albatross.Config.Core;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Hosting.Utility {
	/// <summary>
	/// The utility base acts as the bootstrapper for dependency injections.
	/// </summary>
	/// <typeparam name="Option"></typeparam>
	public abstract class UtilityBase<Option> : IUtility<Option> {
		public TextWriter Out => System.Console.Out;
		public TextWriter Error => System.Console.Error;

		public Option Options { get; }
		public Microsoft.Extensions.Logging.ILogger logger;
		protected IServiceProvider Provider => host.Services;
		protected IHost host;
		private Serilog.Core.Logger serilogLogger;

		protected virtual void ConfigureLogging(LoggerConfiguration cfg) {
			SetupSerilog.UseConsole(cfg, LogEventLevel.Debug);
		}

		public virtual string CurrentDirectory => System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location)!;

		public UtilityBase(Option option) {
			this.Options = option;
			serilogLogger = new SetupSerilog().Configure(ConfigureLogging).Create();

			var env = EnvironmentSetting.DOTNET_ENVIRONMENT;
			IHostBuilder hostBuilder = Host.CreateDefaultBuilder().UseSerilog();
			var configBuilder = new ConfigurationBuilder()
				.SetBasePath(CurrentDirectory)
				.AddJsonFile("appsettings.json", false, true);
			if (!string.IsNullOrEmpty(env.Value)) { configBuilder.AddJsonFile($"appsettings.{env.Value}.json", true, true); }
			var configuration = configBuilder.AddEnvironmentVariables().Build();
			
			host = hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(configuration);
			}).ConfigureServices((ctx, svc) => RegisterServices(ctx.Configuration, env, svc)).Build();

			logger = host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();
			logger.LogDebug("Logging initialized for {type} instance", this.GetType().Name);
			Init(host.Services.GetRequiredService<IConfiguration>(), host.Services);
		}

		public virtual void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			services.AddConfig<ProgramSetting, GetProgramSetting>();
			services.AddSingleton<EnvironmentSetting>(envSetting);
			services.AddSingleton<Microsoft.Extensions.Logging.ILogger>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("default"));
		}
		public async Task<int> Run() {
			try {
				return await this.RunUtility();
			}catch(Exception err) {
				logger.LogError(err, string.Empty);
				return -1;
			}
		}
		protected abstract Task<int> RunUtility();
		public virtual void Init(IConfiguration configuration, IServiceProvider provider) { }

		public void Dispose() {
			logger.LogDebug("Disposing UtilityBase");
			this.host.Dispose();
			logger.LogDebug("CloseAndFlush Logging");
			serilogLogger.Dispose();
		}
	}
}
