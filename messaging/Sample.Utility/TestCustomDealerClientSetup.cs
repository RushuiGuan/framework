using CommandLine;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("test-custom-dealer-client")]
	public class TestCustomDealerClientSetupOption : MyBaseOption {
	}
	public class TestCustomDealerClientSetup : MyUtilityBase<TestCustomDealerClientSetupOption> {
		public TestCustomDealerClientSetup(TestCustomDealerClientSetupOption option) : base(option) {
		}
		public async Task<int> RunUtility(Proxy.CommandProxyService client, ILogger<TestCustomDealerClientSetup> logger) {
			await client.SubmitSystemCommand(new MyCommand1("test command 1"));
			logger.LogInformation("Existing command");
			return 0;
		}
	}
}
