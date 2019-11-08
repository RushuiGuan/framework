using Albatross.Host.Utility;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace Albatross.Framework.Utility {
	[Verb("logging-test")]
	public class LoggingTestOption {}

	public class LoggingTest: UtilityBase<LoggingTestOption> {
		private ILogger<LoggingTest> logger;

		public LoggingTest(LoggingTestOption option):base(option) {
		}

		public override void Init(IConfiguration configuration, IServiceProvider provider) {
			base.Init(configuration, provider);
			logger = provider.GetService<ILogger<LoggingTest>>();
		}

		public override int Run() {
			logger.LogInformation("test");
			return 1;
		}
	}
}
