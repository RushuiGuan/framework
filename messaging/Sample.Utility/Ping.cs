using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Proxy;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("ping", typeof(Ping))]
	public class PingOptions { }

	public class Ping : MyBaseHandler<PingOptions> {
		public Ping(CommandProxyService commandProxy, IOptions<PingOptions> options, ILogger logger) : base(commandProxy, options, logger) {
		}

		public override async Task<int> InvokeAsync(InvocationContext context) {
			await commandProxy.SubmitAppCommand(new Core.Commands.PingCommand(1));
			return 0;
		}
	}
}