using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Core.Commands;
using Sample.Proxy;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("self-destruct", typeof(RunSelfDestruct))]
	public class RunSelfDestructOption {
		[Option("d")]
		public int? Delay { get; set; }
	}
	public class RunSelfDestruct : MyBaseHandler<RunSelfDestructOption> {
		public RunSelfDestruct(CommandProxyService commandProxy, IOptions<RunSelfDestructOption> options, ILogger logger) : base(commandProxy, options, logger) {
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			var cmd = new SelfDestructCommand() {
				Delay = options.Delay,
			};
			await commandProxy.SubmitSystemCommand(cmd);
			return 0;
		}
	}
}