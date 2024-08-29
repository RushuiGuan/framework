using Albatross.Hosting.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;

using System.CommandLine;
using System.Threading.Tasks;
using System;
using System.IO;

namespace Sample.CommandLine {
	[Verb("my-command", "a sample command", Alias = ["m", "t"])]
	public record class MyCommandOptions {
		[Option(Description = "use the name of the test", Alias = ["n"])]
		public string Name { get; set; } = string.Empty;

		public int Data { get; set; }

		[Option(Description = "second file name", Alias = ["s"])]
		public FileInfo SecondFile { get; set; } = null!;


		[Option(Description = "input file name", Alias = ["x"])]
		public FileInfo MyFile { get; set; } = null!;
	}

	public class MyCommandHandler : ICommandHandler {
		private readonly ILogger<MyCommandHandler> logger;
		private readonly IConsole console;
		private readonly MyCommandOptions myOptions;

		private GlobalOptions GlobalOptions { get; }

		public MyCommandHandler(ILogger<MyCommandHandler> logger, IConsole console, IOptions<GlobalOptions> globalOptions, IOptions<MyCommandOptions> myOptions) {
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
			return Task.FromResult(0);
		}
	}
}
