using Albatross.Hosting.Utility;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Albatross.Framework.Utility {
	[Verb("logging-test")]
	public class LoggingTestOption {}

	public class LoggingTest: MyUtilityBase<LoggingTestOption> {

		public LoggingTest(LoggingTestOption option):base(option) {
		}


		public Task<int> RunUtility() {
			for (int i = 0; i < 10; i++) {
				logger.LogInformation("test: {value}", i);
			}
			return Task.FromResult(1);
		}
	}
}
