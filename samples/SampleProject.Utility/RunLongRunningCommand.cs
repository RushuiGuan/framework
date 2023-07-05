using CommandLine;
using System.Threading.Tasks;
using SampleProject.Proxy;
using Albatross.Messaging.Services;
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
		public async Task<int> RunUtility(ICommandClient client) {
			for (int i = 0; i < Options.Count; i++) {
				await client.Submit(new LongRunningCommand(Options.Duration, i));
			}
			return 0;
		}
	}
}
