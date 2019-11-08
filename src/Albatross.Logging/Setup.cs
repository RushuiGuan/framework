using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace Albatross.Logging {
	public class Setup {
		public Setup() {
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("serilog.json")
				.AddEnvironmentVariables()
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();
		}
	}
}
