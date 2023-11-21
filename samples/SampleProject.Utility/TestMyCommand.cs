using Albatross.Messaging.Commands;
using CommandLine;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("my")]
	public class RunMyCommandOption : MyBaseOption { }
	public class RunMyCommand : MyUtilityBase<RunMyCommandOption> {
		public RunMyCommand(RunMyCommandOption option) : base(option) {
		}
		public Task<int> RunUtility(ICommandClient client) {
			BaseCommand myCmd = new MyCommand(1, "test");
			client.Submit(myCmd);
			return Task.FromResult(0);
		}
	}
}
