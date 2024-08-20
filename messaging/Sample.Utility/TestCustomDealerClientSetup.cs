using Albatross.Logging;
using Albatross.Messaging.Commands;
using CommandLine;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using Serilog;
using Serilog.Core;
using Serilog.Filters;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("test-custom-dealer-client")]
	public class TestCustomDealerClientSetupOption : MyBaseOption {
	}
	public class TestCustomDealerClientSetup : MyUtilityBase<TestCustomDealerClientSetupOption> {
		public TestCustomDealerClientSetup(TestCustomDealerClientSetupOption option) : base(option) {
		}
		protected override void ConfigureLogging(LoggerConfiguration cfg) {
			cfg.MinimumLevel.Information()
			.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
			.WriteTo.Logger(log_cfg => log_cfg.Filter.ByExcluding(Matching.WithProperty<string>(Constants.SourceContextPropertyName, source => source != null && source.EndsWith("message-entry")))
			.WriteTo.Console(outputTemplate: SetupSerilog.DefaultOutputTemplate))
			.WriteTo.Logger(log_cfg => log_cfg.Filter.ByIncludingOnly(Matching.WithProperty<string>(Constants.SourceContextPropertyName, source => source != null && source.EndsWith("message-entry")))
			.WriteTo.Console(outputTemplate: "{SourceContext}:{Message:lj}"))
			.Enrich.FromLogContext();
		}
		protected override bool CustomMessaging => true;

		public async Task<int> RunUtility(ICommandClient client, ILogger<TestCustomDealerClientSetup> logger) {
			await client.Submit(new MyCommand1("test command 1"));
			logger.LogInformation("Existing command");
			return 0;
		}
		public override void Dispose() {
			logger.LogInformation("Disposing utility");
			base.Dispose();
		}
	}
}
