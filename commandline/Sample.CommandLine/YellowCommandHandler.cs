using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;

using System.CommandLine;
using System.Threading.Tasks;
using System;

namespace Sample.CommandLine {
	public class YellowCommandHandler : ICommandHandler {
		private readonly ILogger<YellowCommandHandler> logger;
		private readonly IConsole console;
		private readonly MyCommandOptions myOptions;

		private GlobalOptions GlobalOptions { get; }

		public YellowCommandHandler(ILogger<YellowCommandHandler> logger, IConsole console, IOptions<GlobalOptions> globalOptions, IOptions<MyCommandOptions> myOptions) {
			this.logger = logger;
			this.console = console;
			this.myOptions = myOptions.Value;
			this.GlobalOptions = globalOptions.Value;
		}

		public int Invoke(InvocationContext context) {
			throw new NotSupportedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			logger.LogInformation("i am here");
			logger.LogInformation("global options: {global}", this.GlobalOptions);
			logger.LogInformation("my options: {myOptions}", this.myOptions);
			logger.LogInformation("file input: {myOptions}", this.myOptions.MyFile);
			throw new InvalidOperationException("i am crazy");
		}
	}
}
