﻿using Albatross.Config;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
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
			if(this.Options is BaseOption) {
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
			logger.LogDebug("Logging initialized for {type} instance", this.GetType().Name);
			Init(host.Services.GetRequiredService<IConfiguration>(), host.Services);
		}

		public virtual void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			services.AddConfig<ProgramSetting>();
			services.AddSingleton(typeof(ILogger<>), typeof(LoggerEx<>));
			services.AddSingleton(envSetting);
			services.AddSingleton(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger(this.GetType().FullName??"default"));
		}
		public const string RunUtilityMethod = "RunUtility";

		public async Task<int> Run() {
			try {
				var method = this.GetType().GetMethod(RunUtilityMethod, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
				if (method != null) {
					if (method.ReturnType == typeof(Task<int>)) {
						List<object> list = new List<object>();
						foreach (var param in method.GetParameters()) {
							object value = this.Provider.GetRequiredService(param.ParameterType);
							list.Add(value);
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
			} catch(Exception err) {
				logger.LogError(err, string.Empty);
				return -1;
			}
		}
		public virtual void Init(IConfiguration configuration, IServiceProvider provider) { }

		public void Dispose() {
			logger.LogDebug("Disposing UtilityBase");
			this.host.Dispose();
			logger.LogDebug("CloseAndFlush Logging");
			serilogLogger.Dispose();
		}
	}
}
