using Albatross.Messaging.Commands;
using CommandLine;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("my-cmd1")]
	public class RunMyCommand1Option : MyBaseOption {
		[Option('d', "delay")]
		public int Delay { get; set; }
		
		[Option('t', "text")]
		public string? Text { get; set; }
		
		[Option('e', "error")]
		public bool Error { get; set; }

		[Option("child-count")]
		public int ChildCount { get; set; }
	}
	public class RunMyCommand1 : MyUtilityBase<RunMyCommand1Option> {
		public RunMyCommand1(RunMyCommand1Option option) : base(option) {
		}
		public async Task<int> RunUtility(ICommandClient client) {
			for(int i=0; i<Options.Count; i++) {
				var cmd = new MyCommand1($"test command {i}") {
					Error = Options.Error,
					Delay = Options.Delay,
				};

				if (Options.ChildCount > 0) {
					for (int j = 0; j < Options.ChildCount; j++) {
						cmd.Commands.Add(new MyCommand2($"test command {j}"));
					}
				}
				await client.Submit(cmd);
			}
			return 0;
		}
	}
}
