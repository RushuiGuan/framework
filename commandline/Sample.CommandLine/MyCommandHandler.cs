using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.CommandLine {
	[Verb("my-command", typeof(MyCommandHandler), Description = "My Test Command")]
	public record class MyCommandOptions {
		[Option(Required = false)]
		public int Id { get; set; }

		[Option(Required = false)]
		public string Name { get; set; } = string.Empty;
	}
	public partial class MyCommand : IRequireInitialization {
		public void Init() {
			AddValidator(result => {
				var options = result.Children.Where(x => x.Symbol == this.Option_Id || x.Symbol == this.Option_Name)
					.ToList();
				if (options.Count == 0) {
					result.ErrorMessage = "Either --id or --name is required";
				} else if (options.Count == 2) {
					result.ErrorMessage = "Only one of --id or --name is allowed";
				}
			});
		}
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