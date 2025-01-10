using Albatross.CommandLine;
using Microsoft.Extensions.Options;
using Sample.Proxy;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("command-serialization-error-test", typeof(RunCommandSerializationErrorTestHandler))]
	public class RunCommandSerializationErrorTestOptions {
		[Option("c")]
		public bool Callback { get; set; }
	}
	public class RunCommandSerializationErrorTestHandler : BaseHandler<RunCommandSerializationErrorTestOptions> {
		private readonly CommandProxyService client;

		public RunCommandSerializationErrorTestHandler(CommandProxyService client, IOptions<RunCommandSerializationErrorTestOptions> options) : base(options) {
			this.client = client;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			await client.CommandSerializationErrorTest(options.Callback);
			return 0;
		}
	}
}