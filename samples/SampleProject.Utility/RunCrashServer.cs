using Albatross.Messaging.Commands;
using CommandLine;
using SampleProject.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("crash-server")]
	public class RunCrashServerOption : MyBaseOption {
		[Option('d', "delay")]
		public int Delay { get; set; }
	}
	public class RunCrashServer : MyUtilityBase<RunCrashServerOption> {
		public RunCrashServer(RunCrashServerOption option) : base(option) {
		}
		public async Task<int> RunUtility(ICommandClient client) {
			var commands = new List<object> {
				new SelfDestructCommand() {
					Tick = DateTime.Now.Ticks,
					Delay   = Options.Delay,
				},
				new MyCommand1("test1"),
				new MyCommand1("test2"),
				new MyCommand1("test3"),
			};
			var tasks = await client.SubmitCollection(commands);
			return 0;
		}
	}
}
