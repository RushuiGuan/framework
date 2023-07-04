using CommandLine;
using System.Threading.Tasks;
using SampleProject.Proxy;

namespace SampleProject.Utility {
	[Verb("long-running")]
	public class RunLongRunningCommandOption : MyBaseOption {
		[Option('d', "duration", Required = true)]
		public int Duration { get; set; }
	}
	public class RunLongRunningCommand : MyUtilityBase<RunLongRunningCommandOption> {
		public RunLongRunningCommand(RunLongRunningCommandOption option) : base(option) {
		}
		public async Task<int> RunUtility(RunProxyService svc) {
			var result = await svc.LongRunningCommand(Options.Count, Options.Duration);
			Options.WriteOutput(result);
			return 0;
		}
	}
}
