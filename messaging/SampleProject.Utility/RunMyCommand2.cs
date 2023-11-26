using Albatross.Messaging.Commands;
using CommandLine;
using SampleProject.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("my-cmd2")]
	public class RunMyCommand2Option : MyBaseOption {
		[Option('d', "delay")]
		public int Delay { get; set; }

		[Option('t', "text")]
		public string? Text { get; set; }

		[Option('e', "error")]
		public bool Error { get; set; }

		[Option("child-count")]
		public int ChildCount { get; set; }
	}
	public class RunMyCommand2 : MyUtilityBase<RunMyCommand2Option> {
		public RunMyCommand2(RunMyCommand2Option option) : base(option) {
		}
		public async Task<int> RunUtility(ICommandClient client) {
			var commands = new List<object>();
			for (int i = 0; i < Options.Count; i++) {
				var cmd = new MyCommand2($"test command: {i}") {
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
			await client.SubmitCollection(commands);
			return 0;
		}
	}
}
