using Albatross.Messaging.Commands;
using CommandLine;
using Sample.Messaging.Commands;
using System.Threading.Tasks;

namespace Sample.Messaging.Utility {
	[Verb("bad-cmd")]
	public class RunBadCommandOption : MyBaseOption {	}
	public class RunBadCommand : MyUtilityBase<RunBadCommandOption> {
		public RunBadCommand(RunBadCommandOption option) : base(option) {
		}
		public async Task<int> RunUtility(ICommandClient client) {
			await client.Submit(new MyBadCommand("a", "b"));
			return 0;
		}
	}
}
