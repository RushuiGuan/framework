using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Core.Commands;
using Sample.Proxy;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("bad-cmd", typeof(RunBadCommandHandler))]
	public class RunBadCommandOptions : MyBaseOption {	}
	public class RunBadCommandHandler : BaseHandler<RunBadCommandOptions> {
		private readonly CommandProxyService client;

		public RunBadCommandHandler(CommandProxyService client, IOptions<RunBadCommandOptions> options, ILogger logger) : base(options, logger) {
			this.client = client;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			await client.SubmitSystemCommand(new MyBadCommand("a", "b"));
			return 0;
		}
	}
}
