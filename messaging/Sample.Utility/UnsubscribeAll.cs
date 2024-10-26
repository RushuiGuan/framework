using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Proxy;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {

	[Verb("unsub-all", typeof(UnsubscribeAll))]
	public class UnsubscribeAllOption { }
	public class UnsubscribeAll : BaseHandler<UnsubscribeAllOption> {
		private readonly RunProxyService svc;

		public UnsubscribeAll(RunProxyService svc, IOptions<UnsubscribeAllOption> options, ILogger logger) : base(options, logger) {
			this.svc = svc;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			await svc.UnsubscribeAll();
			return 0;
		}
	}
}