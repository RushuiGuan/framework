using CommandLine;
using Sample.Core.Commands;
using Sample.Proxy;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("ping")]
	public class PingOption : MyBaseOption{	}

	public class Ping: MyUtilityBase<PingOption> {
		public Ping(PingOption option) : base(option) {
		}

		public async Task<int> RunUtility(CommandProxyService svc) {
			await svc.SubmitAppCommand(new PingCommand(1));
			return 0;
		}
	}
}
