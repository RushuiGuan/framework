using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Core.Commands;
using Sample.Proxy;
using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("command-with-result", typeof(RunCommandWithResult))]
	public class RunCommandWithResultOptions {
		[Option("n")]
		public string Name { get; set; } = string.Empty;

		[Option("v")]
		public int? Value { get; set; }

		[Option("c")]
		public bool Callback { get; set; }
	}
	public class RunCommandWithResult : BaseHandler<RunCommandWithResultOptions> {
		private readonly CommandProxyService client;

		public RunCommandWithResult(CommandProxyService client, IOptions<RunCommandWithResultOptions> options) : base(options) {
			this.client = client;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			var cmd = new TestOperationWithResultCommand(options.Name) {
				Value = options.Value ?? new Random().Next(),
				Callback = options.Callback,
			};
			var id = await client.SubmitSystemCommand(cmd);
			writer.WriteLine($"Command submitted with id {id}");
			return 0;
		}
	}
}