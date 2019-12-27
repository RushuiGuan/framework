﻿using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace Albatross.Logging {
	public class SetupSerilog : IDisposable {
		public SetupSerilog() {
			var configuration = new ConfigurationBuilder()
				.SetBasePath(System.IO.Directory.GetCurrentDirectory())
				.AddJsonFile("serilog.json", true, true)
				.AddEnvironmentVariables()
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();
		}

		public void Dispose() {
			Log.CloseAndFlush();
		}
	}
}
