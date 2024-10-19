using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Core.Commands;
using Sample.Core.Commands.MyOwnNameSpace;
using Sample.Proxy;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("my-cmd2", typeof(RunMyCommand2))]
	public class RunMyCommand2Option : MyBaseOption {
		[Option("d")]
		public int Delay { get; set; }

		[Option("t")]
		public string? Text { get; set; }

		[Option("e")]
		public bool Error { get; set; }

		public int ChildCount { get; set; }
	}
	public class RunMyCommand2 : BaseHandler<RunMyCommand2Option> {
		private readonly CommandProxyService client;

		public RunMyCommand2(CommandProxyService client, IOptions<RunMyCommand2Option> options, ILogger logger) : base(options, logger) {
			this.client = client;
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			var commands = new List<ISystemCommand>();
			for (int i = 0; i < options.Count; i++) {
				var cmd = new MyCommand2($"test command: {i}") {
					Error = options.Error,
					Delay = options.Delay,
				};
				commands.Add(cmd);

				if (options.ChildCount > 0) {
					for (int j = 0; j < options.ChildCount; j++) {
						cmd.Commands.Add(new MyCommand1($"test command {j}"));
					}
				}
			}
			foreach (var cmd in commands) {
				await client.SubmitSystemCommand(cmd);
			}
			return 0;
		}
	}
}
