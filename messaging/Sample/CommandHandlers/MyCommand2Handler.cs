using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using System.Threading.Tasks;

namespace Sample.CommandHandlers {
	public record class OperationResult {
		public OperationResult(string name) {
			Name = name;
		}
		public string Name { get; set; }
		public int Value { get; set; }
	}
	public class TestOperationWithResultCommandHandler : BaseCommandHandler<TestOperationWithResultCommand, OperationResult> {
		private readonly CommandContext context;
		private readonly ICommandClient client;
		private readonly ILogger<TestOperationWithResultCommandHandler> logger;

		public TestOperationWithResultCommandHandler(CommandContext context, InternalCommandClient commandClient, ILogger<TestOperationWithResultCommandHandler> logger) {
			this.context = context;
			client = commandClient;
			this.logger = logger;
		}

		public override async Task<OperationResult> Handle(TestOperationWithResultCommand command) {
			await Task.Delay(10);
			return new OperationResult(command.Name) { Value = command.Value * 100 };
		}
	}
}