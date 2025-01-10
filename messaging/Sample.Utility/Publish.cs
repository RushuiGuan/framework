using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Proxy;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("pub", typeof(Publish))]
	public class PublishOptions {
		public int Min { get; set; }
		public int Max { get; set; }
		[Option("t")]
		public string Topic { get; set; } = string.Empty;
	}
	public class Publish : BaseHandler<PublishOptions> {
		private readonly CommandProxyService client;

		public Publish(IOptions<PublishOptions> options, CommandProxyService client) : base(options) {
			this.client = client;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			await client.SubmitAppCommand(new Core.Commands.PublishCommand(options.Topic, options.Min, options.Max));
			return 0;
		}
	}
}