using CommandLine;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using Sample.Core.Commands.MyOwnNameSpace;
using Sample.Proxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("self-destruct")]
	public class RunSelfDestructOption : MyBaseOption {
		[Option('d', "delay")]
		public int Delay { get; set; }
	}
	public class RunSelfDestruct : MyUtilityBase<RunSelfDestructOption> {
		public RunSelfDestruct(RunSelfDestructOption option) : base(option) {
		}
		public async Task<int> RunUtility(CommandProxyService client) {
			var commands = new List<ISystemCommand> {
				new SelfDestructCommand() {
					Tick = DateTime.Now.Ticks,
					Delay = Options.Delay,
				},
				new MyCommand1("test1"),
				new MyCommand1("test2"),
				new MyCommand1("test3"),
			};
			try {
				foreach(var cmd in commands) {
					await client.SubmitSystemCommand(cmd);
				}	
			}catch(TimeoutException err) {
				logger.LogError(err.Message);
			}
			return 0;
		}
	}
}
