using Albatross.Hosting.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.CommandLine;

namespace Sample.CommandLine {
	[Verb("my-command", "a sample command", Alias = ["m", "t"])]
	public record class MyCommandOptions {
		[Option("name", "use the name of the test", Alias = ["n"], Required = true)]
		public string Name { get; set; } = string.Empty;

		[Option("file", "input file name", Alias = ["f"])]
		public FileInfo? FileInput { get; set; }

		[Option("second-file", "second file name", Alias = ["s"])]
		public FileInfo SecondFile { get; set; } = null!;
	}
	public class MyCommandHandler : ICommandHandler {
		private readonly ILogger<MyCommandHandler> logger;
		private readonly IConsole console;
		private readonly MyCommandOptions myOptions;

		private GlobalOptions GlobalOptions { get; }

		public MyCommandHandler(ILogger<MyCommandHandler> logger, IConsole console, IOptions<GlobalOptions> globalOptions , IOptions<MyCommandOptions> myOptions) {
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
			console.WriteLine($"global options: {this.GlobalOptions}");
			console.WriteLine($"my options: {this.myOptions}");
			return Task.FromResult(0);
		}
	}
}
