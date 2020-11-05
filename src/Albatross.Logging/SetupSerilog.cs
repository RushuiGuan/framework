using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;

namespace Albatross.Logging {
	public class SetupSerilog {
		public const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:sszzz} [{Level:w3}] {SourceContext} {Message:lj}{NewLine}{Exception}";
		Action<LoggerConfiguration> configActions = null;

		public SetupSerilog UseConfigFile(string name, string basePath = null) {
			Action<LoggerConfiguration> action = cfg => UseConfigFile(cfg, name, basePath);
			configActions += action;
			return this;
		}

		public SetupSerilog UseConsole(LogEventLevel loggingLevel) {
			Action<LoggerConfiguration> action = cfg => UseConsole(cfg, loggingLevel);
			configActions += action;
			return this;
		}

		public SetupSerilog Configure(Action<LoggerConfiguration> action) {
			this.configActions += action;
			return this;
		}

		public Logger Create(bool setDefault = true) {
			LoggerConfiguration cfg = new LoggerConfiguration();
			configActions?.Invoke(cfg);
			var logger = cfg.CreateLogger();
			logger.Information("Logger created");
			if (setDefault) {
				Log.Logger = logger;
			}
			return logger;
		}

		public static void UseConfigFile(LoggerConfiguration cfg, string name, string basePath) {
			if (string.IsNullOrEmpty(basePath)) {
				basePath = System.IO.Directory.GetCurrentDirectory();
			}
			var configuration = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile(name, false, true)
				.AddEnvironmentVariables()
				.Build();
			cfg.ReadFrom.Configuration(configuration);
		}
		public static void UseConsole(LoggerConfiguration cfg, LogEventLevel loggingLevel) {
			cfg.MinimumLevel.ControlledBy(new LoggingLevelSwitch(loggingLevel))
				.WriteTo
				.Console(outputTemplate: DefaultOutputTemplate)
				.Enrich.FromLogContext();
		}
	}
}
