using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Messaging.Commands;
using System.Threading.Tasks;

namespace Sample.Messaging {
	public class PongCommandHandler : BaseCommandHandler<PongCommand> {
		private readonly ICommandClient commandClient;
		private readonly ILogger<PongCommandHandler> logger;

		public PongCommandHandler(InternalCommandClient commandClient, ILogger<PongCommandHandler> logger) {
			this.commandClient = commandClient;
			this.logger = logger;
		}

		public override Task Handle(PongCommand command) {
			if (command.Round == 0) {
				logger.LogInformation("I won");
			} else {
				logger.LogInformation($"round :{command.Round}");
				commandClient.Submit(new PingCommand(command.Round - 1), true);
			}
			return Task.CompletedTask;
		}
	}
}
