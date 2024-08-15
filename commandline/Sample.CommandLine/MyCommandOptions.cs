using Albatross.Hosting.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.CommandLine;

namespace Sample.CommandLine {
	[Verb("my-command", "a sample command", Alias = ["m", "t"])]
	public record class MyCommandOptions {
		public MyCommandOptions(string name) {
			this.Name = name;
		}

		[Option("name", "use the name of the test", Alias = ["n"], Required = true)]
		public string Name { get; set; }

		[Option("file", "input file name", Alias = ["f"])]
		public FileInfo? FileInput { get; set; }
	}
	public class MyCommandHandler : ICommandHandler {
		private readonly ILogger<MyCommandHandler> logger;
		private readonly IConsole console;
		private GlobalOptions GlobalOptions { get; }

		public MyCommandHandler(ILogger<MyCommandHandler> logger, IConsole console, IOptions<GlobalOptions> globalOptions) {
			this.logger = logger;
			this.console = console;
			this.GlobalOptions = globalOptions.Value;
		}

		public int Invoke(InvocationContext context) {
			throw new NotSupportedException();
		}

		public Task<int> InvokeAsync(InvocationContext context) {
			logger.LogInformation("i am here");
			console.WriteLine(this.GlobalOptions.ToString());
			return Task.FromResult(0);
		}
	}
}
