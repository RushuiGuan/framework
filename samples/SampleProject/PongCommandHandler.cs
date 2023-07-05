using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject {
	public class PongCommandHandler : BaseCommandHandler<PongCommand> {
		private readonly ICommandClient commandClient;
		private readonly ILogger<PongCommandHandler> logger;

		public PongCommandHandler(ICommandClient commandClient, ILogger<PongCommandHandler> logger) {
			this.commandClient = commandClient;
			this.logger = logger;
		}

		public override async Task Handle(PongCommand command, string queue) {
			if (command.Round == 0) {
				logger.LogInformation("I won");
				return;
			} else {
				logger.LogInformation($"round :{command.Round}");
				await commandClient.Submit(new PingCommand(command.Round - 1), true);
			}
		}
	}
}
