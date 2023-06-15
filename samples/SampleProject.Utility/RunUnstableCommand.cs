using CommandLine;
using SampleProject.Proxy;
using System;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("unstable")]
	public class RunUnstableCommandOption : MyBaseOption { }
	public class RunUnstableCommand : MyUtilityBase<RunUnstableCommandOption> {
		public RunUnstableCommand(RunUnstableCommandOption option) : base(option) {
		}
		public async Task<int> RunUtility(RunProxyService svc) {
			for (int i = 0; i < Options.Count; i++) {
				try {
					var result = await svc.Unstable(i);
					Options.WriteOutput(result);
				} catch (Exception err) {
					Options.WriteOutput(err.Message);
				}
			}
			return 0;
		}
	}
}
