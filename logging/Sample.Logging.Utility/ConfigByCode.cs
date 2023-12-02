using Albatross.Hosting.Utility;
using Albatross.Logging;
using CommandLine;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Sample.Logging.Utility {
	[Verb("cfg-by-code")]
	public class ConfigByCodeOption: BaseOption {
	}
	public class ConfigByCode : UtilityBase<ConfigByCodeOption> {
		public ConfigByCode(ConfigByCodeOption option) : base(option) {
		}
		protected override void ConfigureLogging(LoggerConfiguration cfg) {
			cfg.MinimumLevel.Information()
				.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
				.WriteTo.Console(outputTemplate: SetupSerilog.DefaultOutputTemplate)
				.Enrich.FromLogContext();
		}
		public  Task<int> RunUtility(ILogger<ConfigByCode> logger) {
			logger.LogInformation("An info msg");
			logger.LogInformation("A warning msg");
			logger.LogInformation("An err msg");
			return Task.FromResult(0);
		}
	}
}
