using Albatross.Config;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private IServiceProvider Provider => host.Services;
		private IHost host;
		private Serilog.Core.Logger serilogLogger;

		protected virtual void ConfigureLogging(LoggerConfiguration cfg) {
			if (this.Options is BaseOption) {
				(this.Options as BaseOption)?.ConfigureLogging(cfg);
			} else {
				SetupSerilog.UseConsole(cfg, LogEventLevel.Debug);
			}
		}

		public virtual string CurrentDirectory => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0])!;

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
		}

		public virtual void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			services.AddConfig<ProgramSetting>();
			services.AddSingleton(envSetting);
			services.AddSingleton(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger(this.GetType().FullName ?? "default"));
		}
		public const string RunUtilityMethod = "RunUtility";

		public async Task<int> Run() {
			Stopwatch? stopWatch = null;
			try {
				if (this.Options is BaseOption baseOption && baseOption.Benchmark) {
					stopWatch = new Stopwatch();
					stopWatch.Start();
				}
				logger.LogDebug("Logging initialized for {type} instance", this.GetType().Name);
				await Init(host.Services.GetRequiredService<IConfiguration>(), host.Services);
				if (stopWatch != null) {
					logger.LogInformation("Initialization: {time:#,#} ms", stopWatch.ElapsedMilliseconds);
					stopWatch.Restart();
				}
				var method = this.GetType().GetMethod(RunUtilityMethod, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
				if (method != null) {
					if (method.ReturnType == typeof(Task<int>)) {
						List<object> list = new List<object>();
						foreach (var param in method.GetParameters()) {
							object value = this.Provider.GetRequiredService(param.ParameterType);
							list.Add(value);
						}
						if (stopWatch != null) {
							logger.LogInformation("Reflection: {time:#,#} ms", stopWatch.ElapsedMilliseconds);
							stopWatch.Restart();
						}
						var result = method.Invoke(this, list.ToArray());
						if (result == null) {
							logger.LogWarning("RunUtility method has returned a null value.  This is not a good form.  Please always return a non null Task<int> object such as Task.FromResult(0).");
							return 0;
						} else {
							return await (Task<int>)result;
						}
					} else {
						throw new InvalidOperationException("RunUtility method has to return Task<int>");
					}
				} else {
					throw new InvalidOperationException("RunUtility method is not defined in this class");
				}
			} catch (Exception err) {
				logger.LogError(err, string.Empty);
				return -1;
			} finally {
				if (stopWatch != null) {
					logger.LogInformation("Execution: {time:#,#} ms", stopWatch.ElapsedMilliseconds);
					stopWatch.Restart();
				}
			}
		}
		public virtual Task Init(IConfiguration configuration, IServiceProvider provider) => Task.CompletedTask;
		public virtual void Dispose() {
			logger.LogDebug("Disposing UtilityBase");
			this.host.Dispose();
			logger.LogDebug("CloseAndFlush Logging");
			serilogLogger.Dispose();
		}
	}
}
