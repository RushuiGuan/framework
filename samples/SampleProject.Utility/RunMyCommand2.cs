using Albatross.Messaging.Commands;
using CommandLine;
using SampleProject.Commands;
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
			for(int i=0; i<Options.Count; i++) {
				var cmd = new MyCommand2 {
					Error = Options.Error,
					Text = Options.Text ?? $"test command: {i}",
				};

				if (Options.ChildCount > 0) {
					for (int j = 0; j < Options.ChildCount; j++) {
						cmd.Commands.Add(new MyCommand1() {
							Text = $"child command {j} of {i}",
						});
					}
				}
				await client.Submit(cmd);
			}
			return 0;
		}
	}
}
