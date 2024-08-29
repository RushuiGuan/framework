using Albatross.Hosting.Utility;
using CommandLine;
using Sample.Core.Commands;
using Sample.Proxy;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("pub")]
	public class PublishOption : BaseOption {
		[Option("min", Required = true)]
		public int Min { get; set; }

		[Option("max", Required = true)]
		public int Max { get; set; }

		[Option('t', "topic", Required = true)]
		public string Topic { get; set; } = string.Empty;
	}
	public class Publish : MyUtilityBase<PublishOption> {
		public Publish(PublishOption option) : base(option) {
		}
		public async Task<int> RunUtility(CommandProxyService client) {
			await client.SubmitAppCommand(new PublishCommand(Options.Topic, Options.Min, Options.Max));
			return 0;
		}
	}
}
