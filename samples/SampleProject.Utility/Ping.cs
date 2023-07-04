using CommandLine;
using SampleProject.Proxy;
using System.Threading.Tasks;

namespace SampleProject.Utility {
	[Verb("ping")]
	public class PingOption : MyBaseOption{	}

	public class Ping: MyUtilityBase<PingOption> {
		public Ping(PingOption option) : base(option) {
		}

		public async Task<int> RunUtility(RunProxyService svc) {
			await svc.Ping();
			return 0;
		}
	}
}
