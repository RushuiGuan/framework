using Albatross.CommandLine;
using Microsoft.Extensions.Options;
using Sample.Proxy;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {

	[Verb("sub", typeof(Subscribe))]
	public class SubscribeOption {
		[Option("o")]
		public bool On { get; set; }

		[Option("t")]
		public string Topic { get; set; } = string.Empty;
	}
	public class Subscribe : BaseHandler<SubscribeOption> {
		private readonly RunProxyService svc;

		public Subscribe(RunProxyService svc, IOptions<SubscribeOption> options) : base(options) {
			this.svc = svc;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			if (options.On) {
				await svc.Subscribe(options.Topic);
			} else {
				await svc.Unsubscribe(options.Topic);
			}
			return 0;
		}
	}
}