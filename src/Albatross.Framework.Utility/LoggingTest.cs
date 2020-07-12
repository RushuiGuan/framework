using Albatross.Host.Utility;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Albatross.Framework.Utility {
	[Verb("logging-test")]
	public class LoggingTestOption {}

	public class LoggingTest: UtilityBase<LoggingTestOption> {

		public LoggingTest(LoggingTestOption option):base(option) {
		}


		public override Task<int> RunAsync() {
			logger.LogInformation("test");
			return Task.FromResult(1);
		}
	}
}
