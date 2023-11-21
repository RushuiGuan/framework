using Albatross.Messaging.Commands;
using CommandLine;
using SampleProject.Commands;
using SampleProject.Proxy;
using System;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("unstable")]
	public class RunUnstableCommandOption : MyBaseOption { }
	public class RunUnstableCommand : MyUtilityBase<RunUnstableCommandOption> {
		public RunUnstableCommand(RunUnstableCommandOption option) : base(option) {
		}
		public Task<int> RunUtility(ICommandClient client) {
			for (int i = 0; i < Options.Count; i++) {
				try {
					var result = client.Submit(new UnstableCommand(i));
					Options.WriteOutput(result);
				} catch (Exception err) {
					Options.WriteOutput(err.Message);
				}
			}
			return Task.FromResult(0);
		}
	}
}
