﻿using Albatross.Config;
using Albatross.Messaging.Commands;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Messaging.Commands;
using Sample.Messaging.Proxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Messaging.Utility {
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
		protected override bool CustomMessaging => true;
		public RunWithCallback(RunWithCallbackOption option) : base(option) { }

		public async Task<int> RunUtility(ICommandClient client) {
			var commands = new List<object>();
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
			await client.SubmitCollection(commands, false);
			Console.ReadLine();
			return 0;
		}
	}
}
