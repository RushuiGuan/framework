using CommandLine;
using Sample.Core.Commands;
using Sample.Proxy;
using System.Threading.Tasks;

namespace Sample.Utility {
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
		public async Task<int> RunUtility( CommandProxyService client) {
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
				await client.SubmitSystemCommand(cmd);
			}
			return 0;
		}
	}
}
