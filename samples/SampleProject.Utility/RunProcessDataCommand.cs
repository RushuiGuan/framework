using Albatross.Messaging.Commands;
using CommandLine;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("process-data")]
	public class RunProcessDataCommandOption : MyBaseOption { }
	public class RunProcessDataCommand : MyUtilityBase<RunProcessDataCommandOption> {
		public RunProcessDataCommand(RunProcessDataCommandOption option) : base(option) { }
		public async Task<int> RunUtility(ICommandClient client) {
			for (int i = 0; i < Options.Count; i++) {
				var result = await client.Submit<ProcessDataCommand, long>(new ProcessDataCommand(i));
				Options.WriteOutput(result);
			}
			return 0;
		}
	}
}
