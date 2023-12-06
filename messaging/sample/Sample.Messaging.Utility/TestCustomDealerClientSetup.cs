using Albatross.Config;
using Albatross.Hosting.Utility;
using Albatross.Logging;
using Albatross.Messaging.Commands;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sample.Messaging.Commands;
using Serilog;
using Serilog.Core;
using Serilog.Filters;
using System;
using System.Threading.Tasks;

namespace Sample.Messaging.Utility {
	[Verb("test-custom-dealer-client")]
	public class TestCustomDealerClientSetupOption : MyBaseOption {
	}
	public class TestCustomDealerClientSetup : UtilityBase<TestCustomDealerClientSetupOption> {
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
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.AddCustomMessagingClient();
		}
		public override async Task Init(IConfiguration configuration, IServiceProvider provider) {
			await provider.UseCustomMessagingClient(logger);
		}

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
