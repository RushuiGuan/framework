using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Core.Commands;
using Sample.Proxy;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("test-custom-dealer-client", typeof(TestCustomDealerClientSetup))]
	public class TestCustomDealerClientSetupOption : MyBaseOption {
	}
	public class TestCustomDealerClientSetup : MyBaseHandler<TestCustomDealerClientSetupOption> {
		public TestCustomDealerClientSetup(CommandProxyService commandProxy, IOptions<TestCustomDealerClientSetupOption> options, ILogger<TestCustomDealerClientSetup> logger) : base(commandProxy, options, logger) {
		}

		public override async Task<int> InvokeAsync(InvocationContext context) {
			await commandProxy.SubmitSystemCommand(new MyCommand1("test command 1"));
			logger.LogInformation("Existing command");
			return 0;
		}
	}
}