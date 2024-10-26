using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Core.Commands;
using Sample.Proxy;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("command-handler-exception-test", typeof(RunCommandHandlerExceptionTest))]
	public class RunCommandHandlerExceptionTestOptions {
		[Option("d")]
		public int? Delay { get; set; }
		[Option("c")]
		public bool Callback { get; set; }
	}
	public class RunCommandHandlerExceptionTest : MyBaseHandler<RunCommandHandlerExceptionTestOptions> {
		public RunCommandHandlerExceptionTest(CommandProxyService commandProxy, IOptions<RunCommandHandlerExceptionTestOptions> options, ILogger logger) : base(commandProxy, options, logger) {
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			var cmd = new CommandHandlerExceptionTestCommand {
				Delay = options.Delay ?? 0,
				Callback = options.Callback,
			};
			await this.commandProxy.SubmitSystemCommand(cmd);
			return 0;
		}
	}
}