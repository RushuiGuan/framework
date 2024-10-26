using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using System.Threading.Tasks;

namespace Sample.CommandHandlers {
	public class PingCommandHandler : BaseCommandHandler<PingCommand> {
		private readonly ICommandClient commandClient;
		private readonly ILogger<PingCommandHandler> logger;

		public PingCommandHandler(InternalCommandClient commandClient, ILogger<PingCommandHandler> logger) {
			this.commandClient = commandClient;
			this.logger = logger;
		}

		public override Task Handle(PingCommand command) {
			if (command.Round == 0) {
				logger.LogInformation("I won");
			} else {
				logger.LogInformation($"round :{command.Round}");
				var id = commandClient.Submit(new PongCommand(command.Round - 1), true);
			}
			return Task.CompletedTask;
		}
	}
}