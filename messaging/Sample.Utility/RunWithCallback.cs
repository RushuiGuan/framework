using CommandLine;
using Sample.Core.Commands;
using Sample.Core.Commands.MyOwnNameSpace;
using Sample.Proxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("callback-cmd")]
	public class RunWithCallbackOption : MyBaseOption {
		[Option('d', "delay")]
		public int Delay { get; set; }

		[Option('t', "text")]
		public string? Text { get; set; }

		[Option('e', "error")]
		public bool Error { get; set; }

		[Option("child-count")]
		public int ChildCount { get; set; }
	}
	public class RunWithCallback : MyUtilityBase<RunWithCallbackOption> {
		public RunWithCallback(RunWithCallbackOption option) : base(option) { }

		public async Task<int> RunUtility(CommandProxyService client) {
			var commands = new List<ISystemCommand>();
			for (int i = 0; i < Options.Count; i++) {
				var cmd = new MyCommand3($"test command: {i}") {
					Error = Options.Error,
					Delay = Options.Delay,
				};
				commands.Add(cmd);

				if (Options.ChildCount > 0) {
					for (int j = 0; j < Options.ChildCount; j++) {
						cmd.Commands.Add(new MyCommand1($"test command {j}"));
					}
				}
			}
			foreach (var cmd in commands) {
				await client.SubmitSystemCommand(cmd);
			}
			Console.ReadLine();
			return 0;
		}
	}
}
