using Albatross.Messaging.Commands;
using CommandLine;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("self-destruct")]
	public class RunSelfDestructOption : MyBaseOption {
		[Option('d', "delay")]
		public int Delay { get; set; }
	}
	public class RunSelfDestruct : MyUtilityBase<RunSelfDestructOption> {
		public RunSelfDestruct(RunSelfDestructOption option) : base(option) {
		}
		public async Task<int> RunUtility(ICommandClient client) {
			var commands = new List<object> {
				new SelfDestructCommand() {
					Tick = DateTime.Now.Ticks,
					Delay = Options.Delay,
				},
				new MyCommand1("test1"),
				new MyCommand1("test2"),
				new MyCommand1("test3"),
			};
			try {
				await client.SubmitCollection(commands);
			}catch(TimeoutException err) {
				logger.LogError(err.Message);
			}
			return 0;
		}
	}
}
