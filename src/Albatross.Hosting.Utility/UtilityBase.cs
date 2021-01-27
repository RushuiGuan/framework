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
using System.Text.Json;
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

		public virtual string CurrentDirectory => System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);

		public UtilityBase(Option option) {
			this.Options = option;
			serilogLogger = new SetupSerilog().Configure(ConfigureLogging).Create();

			var env = System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")?.ToLower();
			IHostBuilder hostBuilder = Host.CreateDefaultBuilder().UseSerilog();
			var configBuilder = new ConfigurationBuilder()
				.SetBasePath(CurrentDirectory)
				.AddJsonFile("appsettings.json", false, true);
			if (!string.IsNullOrEmpty(env)) { configBuilder.AddJsonFile($"appsettings.{env}.json", true, true); }
			var configuration = configBuilder.AddEnvironmentVariables().Build();
			
			host = hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(configuration);
			}).ConfigureServices((ctx, svc) => RegisterServices(ctx.Configuration, svc)).Build();

			logger = host.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();
			logger.LogInformation("Logging initialized for {type} instance", this.GetType().Name);
			Init(host.Services.GetRequiredService<IConfiguration>(), host.Services);
		}

		public virtual void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			services.AddConfig<ProgramSetting, GetProgramSetting>();
			services.AddSingleton<Microsoft.Extensions.Logging.ILogger>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("default"));
		}
		public abstract Task<int> RunAsync();
		public virtual void Init(IConfiguration configuration, IServiceProvider provider) { }

		public void Dispose() {
			logger.LogDebug("Disposing UtilityBase");
			this.host.Dispose();
			logger.LogDebug("CloseAndFlush Logging");
			serilogLogger.Dispose();
		}

		protected EntityType ReadInput<EntityType>(string file) {
			using var reader = new StreamReader(file);
			string text = reader.ReadToEnd();
			return JsonSerializer.Deserialize<EntityType>(text, new JsonSerializerOptions {
				IgnoreNullValues = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true
			});
		}
	}
}
