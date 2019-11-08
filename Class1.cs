using Serilog;
using System;

namespace Albatross.Logging {
	public class SetupLogging {
		public SetupLogging() {
			Log.Logger = new LoggerConfiguration()
					.Enrich.FromLogContext()
					.WriteTo.Console()
					.CreateLogger();
		}
	}
}
