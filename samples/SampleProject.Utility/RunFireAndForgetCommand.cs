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
		public async Task<int> RunUtility(ICommandClient client) {
			for (int i = 0; i < Options.Count; i++) {
				await client.Submit(new FireAndForgetCommand(i, Options.Duration));
			}
			return 0;
		}
	}
}
