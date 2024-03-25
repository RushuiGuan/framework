using Albatross.Hosting.Utility;
using CommandLine;
using Sample.Messaging.Proxy;
using System.Threading.Tasks;

namespace Sample.Messaging.Utility {

	[Verb("unsub-all")]
	public class UnsubscribeAllOption : BaseOption { }
	public class UnsubscribeAll : MyUtilityBase<UnsubscribeAllOption> {
		public UnsubscribeAll(UnsubscribeAllOption option) : base(option) {
		}
		public async Task<int> RunUtility(RunProxyService svc) {
			await svc.UnsubscribeAll();
			return 0;
		}
	}
}
