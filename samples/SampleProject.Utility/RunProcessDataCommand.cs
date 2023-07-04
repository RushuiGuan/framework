using CommandLine;
using SampleProject.Proxy;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("process-data")]
	public class RunProcessDataCommandOption : MyBaseOption { }
	public class RunProcessDataCommand : MyUtilityBase<RunProcessDataCommandOption> {
		public RunProcessDataCommand(RunProcessDataCommandOption option) : base(option) { }
		public async Task<int> RunUtility(RunProxyService svc) {
			for (int i = 0; i < Options.Count; i++) {
				var result = await svc.ProcessData(i);
				Options.WriteOutput(result);
			}
			return 0;
		}
	}
}
