using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;

namespace Albatross.Logging {
	public class SetupSerilog : IDisposable {
		public const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:sszzz} [{Level:w3}] {Properties:j} {Message:lj}{NewLine}{Exception}";


		public void UseConfigFile(string name) {
			string basePath = System.IO.Directory.GetCurrentDirectory();

			var configuration = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile(name, false, true)
				.AddEnvironmentVariables()
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

			Log.Information("Logger Created at root path {basePath}", basePath);
		}

		public void UseConsoleAndFile(LogEventLevel loggingLevel, string fileName) {
			var cfg = new LoggerConfiguration()
				.MinimumLevel.ControlledBy(new LoggingLevelSwitch(loggingLevel))
				.WriteTo.Console(outputTemplate: DefaultOutputTemplate);

			if (!string.IsNullOrWhiteSpace(fileName)) { cfg.WriteTo.File(fileName, outputTemplate: DefaultOutputTemplate); }
			cfg.Enrich.FromLogContext();
			Log.Logger = cfg.CreateLogger();
			Log.Information("Logger created"); ;
		}

		public void Dispose() {
			Log.Information("Logger Closing");
			Log.CloseAndFlush();
		}
	}
}
