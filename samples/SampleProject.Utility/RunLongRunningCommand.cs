using CommandLine;
using System.Threading.Tasks;
using SampleProject.Commands;
using Albatross.Messaging.Commands;

namespace SampleProject.Utility {
	[Verb("long-running")]
	public class RunLongRunningCommandOption : MyBaseOption {
		[Option('d', "duration", Required = true)]
		public int Duration { get; set; }
	}
	public class RunLongRunningCommand : MyUtilityBase<RunLongRunningCommandOption> {
		public RunLongRunningCommand(RunLongRunningCommandOption option) : base(option) {
		}
		public Task<int> RunUtility(ICommandClient client) {
			for (int i = 0; i < Options.Count; i++) {
				client.Submit(new LongRunningCommand(Options.Duration, i));
			}
			return Task.FromResult(0);
		}
	}
}
