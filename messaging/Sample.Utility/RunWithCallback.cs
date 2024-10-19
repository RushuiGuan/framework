using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Core.Commands;
using Sample.Core.Commands.MyOwnNameSpace;
using Sample.Proxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("callback-cmd", typeof(RunWithCallback))]
	public class RunWithCallbackOption : MyBaseOption {
		[Option("d")]
		public int Delay { get; set; }

		[Option("t")]
		public string? Text { get; set; }

		[Option("e")]
		public bool Error { get; set; }

		public int ChildCount { get; set; }
	}
	public class RunWithCallback : MyBaseHandler<RunWithCallbackOption> {
		public RunWithCallback(CommandProxyService commandProxy, IOptions<RunWithCallbackOption> options, ILogger logger) : base(commandProxy, options, logger) {
		}

		public async Task<int> RunUtility(CommandProxyService client) {
			var commands = new List<ISystemCommand>();
			for (int i = 0; i < options.Count; i++) {
				var cmd = new MyCommand3($"test command: {i}") {
					Error = options.Error,
					Delay = options.Delay,
				};
				commands.Add(cmd);

				if (options.ChildCount > 0) {
					for (int j = 0; j < options.ChildCount; j++) {
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
