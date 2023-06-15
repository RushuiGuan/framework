using Albatross.Hosting.Utility;
using CommandLine;
using SampleProject.Proxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleProject.Utility {

	[Verb("sub")]
	public class SubscribeOption : BaseOption {
		[Option('o')]
		public bool On { get; set; }

		[Option('t', Required = true)]
		public IEnumerable<string> Topic { get; set; } = Array.Empty<string>();
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
