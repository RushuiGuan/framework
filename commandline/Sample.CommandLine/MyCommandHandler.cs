using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Sample.CommandLine {
	[Verb("my-command", typeof(MyCommandHandler), Description = "My Test Command")]
	public record class MyCommandOptions {
		public string Name { get; set; } = string.Empty;
	}
	public class MyCommandHandler : ICommandHandler {
		private readonly ILogger logger;
		private readonly MyCommandOptions options;

		public MyCommandHandler(ILogger logger, IOptions<MyCommandOptions> options) {
			this.logger = logger;
			this.options = options.Value;
		}
		// this method is not used by System.CommandLine
		public int Invoke(InvocationContext context) => throw new System.NotSupportedException();
		public Task<int> InvokeAsync(InvocationContext context) {
			logger.LogInformation("Command {name }is invoked with parameter of {param}", context.ParsedCommandName(), options.Name);
			return Task.FromResult(0);
		}
	}
}