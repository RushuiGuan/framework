using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Core.Commands;
using Sample.Core.Commands.MyOwnNameSpace;
using Sample.Proxy;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("self-destruct", typeof(RunSelfDestruct))]
	public class RunSelfDestructOption : MyBaseOption {
		[Option("d")]
		public int Delay { get; set; }
	}
	public class RunSelfDestruct : MyBaseHandler<RunSelfDestructOption> {
		public RunSelfDestruct(CommandProxyService commandProxy, IOptions<RunSelfDestructOption> options, ILogger logger) : base(commandProxy, options, logger) {
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			var commands = new List<ISystemCommand> {
				new SelfDestructCommand() {
					Tick = DateTime.Now.Ticks,
					Delay =  options.Delay,
				},
				new MyCommand1("test1"),
				new MyCommand1("test2"),
				new MyCommand1("test3"),
			};
			try {
				foreach (var cmd in commands) {
					await commandProxy.SubmitSystemCommand(cmd);
				}
			} catch (TimeoutException err) {
				logger.LogError(err.Message);
			}
			return 0;
		}
	}
}
