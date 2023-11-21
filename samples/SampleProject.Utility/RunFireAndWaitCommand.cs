using CommandLine;
using System.Threading.Tasks;
using Albatross.Messaging.Commands;
using SampleProject.Commands;

namespace SampleProject.Utility {
	[Verb("fire-and-wait")]
	public class RunFireAndWaitCommandOption : MyBaseOption {
		[Option('d', "duration")]
		public int? Duration { get; set; }
	}
	public class RunFireAndWaitCommand : MyUtilityBase<RunFireAndWaitCommandOption> {
		public RunFireAndWaitCommand(RunFireAndWaitCommandOption option) : base(option) {
		}
		public Task<int> RunUtility(ICommandClient client) {
			for (int i = 0; i < Options.Count; i++) {
				client.Submit(new FireAndForgetCommand(i, Options.Duration), false);
			}
			return Task.FromResult(0);
		}
	}
}
