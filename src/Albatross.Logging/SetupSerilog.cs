using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;

namespace Albatross.Logging {
	public class SetupSerilog : IDisposable {
		public const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:sszzz} [{Level:w3}] {Properties:j} {Message:lj}{NewLine}{Exception}";


		public void UseConfigFile(string name) {
			var configuration = new ConfigurationBuilder()
				.SetBasePath(System.IO.Directory.GetCurrentDirectory())
				.AddJsonFile(name, true, true)
				.AddEnvironmentVariables()
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();
		}

		public void UseConsoleAndFile(LogEventLevel loggingLevel, string fileName) {
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.ControlledBy(new LoggingLevelSwitch(loggingLevel))
				.WriteTo.Console(outputTemplate: DefaultOutputTemplate)
				.WriteTo.File(fileName, outputTemplate:DefaultOutputTemplate)
				.Enrich.FromLogContext()
				.CreateLogger();
		}

		public void Dispose() {
			Log.CloseAndFlush();
		}
	}
}
