using CommandLine;
using System.Threading.Tasks;
using SampleProject.Proxy;

namespace SampleProject.Utility {
	[Verb("fire-and-wait")]
	public class RunFireAndWaitCommandOption : MyBaseOption {
		[Option('d', "duration")]
		public int? Duration { get; set; }
	}
	public class RunFireAndWaitCommand : MyUtilityBase<RunFireAndWaitCommandOption> {
		public RunFireAndWaitCommand(RunFireAndWaitCommandOption option) : base(option) {
		}
		public async Task<int> RunUtility(RunProxyService svc) {
			await svc.FireAndWait(Options.Count, Options.Duration);
			return 0;
		}
	}
}
