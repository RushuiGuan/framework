﻿using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;

namespace Albatross.Logging {
	public class SetupSerilog {
		public const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:sszzz} [{Level:w3}] {SourceContext} {Message:lj}{NewLine}{Exception}";
		Action<LoggerConfiguration>? configActions = null;

		public SetupSerilog UseConfigFile(string environment, string? basePath, string[]? commandLineArgs) {
			Action<LoggerConfiguration> action = cfg => UseConfigFile(cfg, environment, basePath, commandLineArgs);
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
			if (setDefault) {
				Log.Logger = logger;
			}
			return logger;
		}

		public static void UseConfigFile(LoggerConfiguration cfg, string environment, string? basePath, string[]? commandlineArgs) {
			if (string.IsNullOrEmpty(basePath)) {
				basePath = AppContext.BaseDirectory;
			}
			var configBuilder = new ConfigurationBuilder()
				.SetBasePath(basePath!)
				.AddJsonFile("serilog.json", false, true);
			if (!string.IsNullOrEmpty(environment)) { configBuilder.AddJsonFile($"serilog.{environment}.json", true, true); }
			configBuilder.AddEnvironmentVariables().AddCommandLine(commandlineArgs ?? new string[0]);
			var configuration = configBuilder.Build();
			cfg.ReadFrom.Configuration(configuration);
		}

		private static LoggingLevelSwitch consoleLoggingLevelSwitch = new LoggingLevelSwitch();
		public static void UseConsole(LoggerConfiguration cfg, LogEventLevel? loggingLevel) {
			if (loggingLevel != null) {
				consoleLoggingLevelSwitch.MinimumLevel = loggingLevel.Value;
			}
			cfg.MinimumLevel.ControlledBy(consoleLoggingLevelSwitch)
				.WriteTo
				.Console(outputTemplate: DefaultOutputTemplate)
				.Enrich.FromLogContext();
		}
		public static void SwitchConsoleLoggingLevel(LogEventLevel loggingLevel) {
			consoleLoggingLevelSwitch.MinimumLevel = loggingLevel;
		}
	}
}