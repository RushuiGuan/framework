using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace Albatross.Logging {
	public class SetupSerilog : IDisposable {
		public SetupSerilog() {
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("serilog.json", false, true)
				.AddEnvironmentVariables()
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();
		}

		public void Dispose() {

		}
	}
}
