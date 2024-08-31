using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.CommandLine;
using System.Threading.Tasks;
using System;

namespace Sample.CommandLine {
	[Verb("sys-command", Alias = ["t"])]
	public record class SysCommandOptions {
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		[Option(Required = false)]
		public decimal Price { get; set; }
	}

	public class SysCommandHandler : ICommandHandler {
		private readonly ILogger<SysCommandHandler> logger;
		private readonly IConsole console;
		private readonly SysCommandOptions myOptions;

		public SysCommandHandler(ILogger<SysCommandHandler> logger, IConsole console, IOptions<SysCommandOptions> myOptions) {
			this.logger = logger;
			this.console = console;
			this.myOptions = myOptions.Value;
		}

		public int Invoke(InvocationContext context) {
			throw new NotSupportedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			logger.LogInformation("i am here");
			logger.LogInformation("my options: {myOptions}", this.myOptions);
			return Task.FromResult(0);
		}
	}
}
