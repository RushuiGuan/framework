using CommandLine;
using Sample.Core.Commands;
using Sample.Proxy;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("bad-cmd")]
	public class RunBadCommandOption : MyBaseOption {	}
	public class RunBadCommand : MyUtilityBase<RunBadCommandOption> {
		public RunBadCommand(RunBadCommandOption option) : base(option) {
		}
		public async Task<int> RunUtility(CommandProxyService client) {
			await client.SubmitSystemCommand(new MyBadCommand("a", "b"));
			return 0;
		}
	}
}
