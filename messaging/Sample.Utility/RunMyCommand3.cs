using CommandLine;
using Sample.Core.Commands;
using Sample.Core.Commands.MyOwnNameSpace;
using Sample.Proxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("my-cmd3")]
	public class RunMyCommand3Option : MyBaseOption {
		[Option('d', "delay")]
		public int Delay { get; set; }

		[Option('t', "text")]
		public string? Text { get; set; }

		[Option('e', "error")]
		public bool Error { get; set; }

		[Option("child-count")]
		public int ChildCount { get; set; }
	}
	public class RunMyCommand3 : MyUtilityBase<RunMyCommand3Option> {
		public RunMyCommand3(RunMyCommand3Option option) : base(option) {
		}
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
			foreach(var cmd in commands) {
				await client.SubmitSystemCommand(cmd);
			}
			Console.ReadLine();
			return 0;
		}
	}
}
