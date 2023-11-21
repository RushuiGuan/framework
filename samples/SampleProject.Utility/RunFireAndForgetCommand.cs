using CommandLine;
using System.Threading.Tasks;
using Albatross.Messaging.Commands;
using SampleProject.Commands;

namespace SampleProject.Utility {
	[Verb("fire-and-forget")]
	public class RunFireAndForgetCommandOption : MyBaseOption {
		[Option('d', "duration")]
		public int? Duration { get; set; }
	}
	public class RunFireAndForgetCommand : MyUtilityBase<RunFireAndForgetCommandOption> {
		public RunFireAndForgetCommand(RunFireAndForgetCommandOption option) : base(option) {
		}
		public Task<int> RunUtility(ICommandClient client) {
			for (int i = 0; i < Options.Count; i++) {
				client.Submit(new FireAndForgetCommand(i, Options.Duration));
			}
			return Task.FromResult(0);
		}
	}
}
