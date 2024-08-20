using Albatross.Hosting.Utility;
using CommandLine;
using Sample.Proxy;
using System;
using System.Threading.Tasks;

namespace Sample.Utility {

	[Verb("sub")]
	public class SubscribeOption : BaseOption {
		[Option('o', "on")]
		public bool On { get; set; }

		[Option('t', "topic", Required = true)]
		public string Topic { get; set; } = string.Empty;
	}
	public class Subscribe : MyUtilityBase<SubscribeOption> {
		public Subscribe(SubscribeOption option) : base(option) {
		}
		public async Task<int> RunUtility(RunProxyService svc) {
			if (Options.On) {
				await svc.Subscribe(Options.Topic);
			} else {
				await svc.Unsubscribe(Options.Topic);
			}
			return 0;
		}
	}
}
