using CommandLine;
using SampleProject.Proxy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("do-math")]
	public class RunAddCommandOption : MyBaseOption { }

	public class RunDoMathCommand : MyUtilityBase<RunAddCommandOption> {
		public RunDoMathCommand(RunAddCommandOption option) : base(option) {
		}
		public async Task<int> RunUtility(RunProxyService svc) {

			ISet<Task<long>> list = new HashSet<Task<long>>();
			for (int i = 0; i < Options.Count; i++) {
				var task = svc.DoMathWork(i);
				list.Add(task);
			}
			Options.WriteOutput($"total task count: {list.Count}");
			do {
				var task = await Task.WhenAny(list);
				Options.WriteOutput(task.Result);
				list.Remove(task);
				Options.WriteOutput($"remaining task count: {list.Count}");
			} while(list.Count > 0);
			Options.WriteOutput("exiting program");
			return 0;
		}
	}
}
