using Albatross.Messaging.Commands;
using CommandLine;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("kick-off")]
	public class KickOffDoNothingOption : MyBaseOption { }

	public class KickOffDoNothing : MyUtilityBase<KickOffDoNothingOption> {
		public KickOffDoNothing(KickOffDoNothingOption option) : base(option) {
		}
		public async Task<int> RunUtility(ICommandClient client) {
			for(int i=0; i<100; i++) {
				await client.Submit(new KickOffDoNothingCommand());
			}
			return 0;
		}
	}
}
