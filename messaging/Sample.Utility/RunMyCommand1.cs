using System.CommandLine;
using Sample.Core.Commands;
using Sample.Proxy;
using System.Threading.Tasks;
using Albatross.CommandLine;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.CommandLine.Invocation;

namespace Sample.Utility {
	[Verb("my-cmd1", typeof(RunMyCommand1))]
	public class RunMyCommand1Option : MyBaseOption {
		[Option("d")]
		public int Delay { get; set; }

		[Option("t")]
		public string? Text { get; set; }

		[Option("e")]
		public bool Error { get; set; }

		[Option("child-count")]
		public int ChildCount { get; set; }
	}
	public class RunMyCommand1 : BaseHandler<RunMyCommand1Option> {
		private readonly CommandProxyService client;

		public RunMyCommand1(CommandProxyService client, IOptions<RunMyCommand1Option> options, ILogger logger) : base(options, logger) {
			this.client = client;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			for (int i = 0; i < options.Count; i++) {
				var cmd = new MyCommand1($"test command {i}") {
					Error = options.Error,
					Delay = options.Delay,
				};
				await client.SubmitSystemCommand(cmd);
			}
			return 0;
		}
	}
}
