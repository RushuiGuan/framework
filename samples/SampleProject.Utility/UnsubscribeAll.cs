using Albatross.Hosting.Utility;
using CommandLine;
using SampleProject.Proxy;
using System.Threading.Tasks;

namespace SampleProject.Utility {

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
