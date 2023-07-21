using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject {
	public class PingCommandHandler : BaseCommandHandler<PingCommand> {
		private readonly ICommandClient commandClient;
		private readonly ILogger<PingCommandHandler> logger;

		public PingCommandHandler(InternalCommandClient commandClient, ILogger<PingCommandHandler> logger) {
			this.commandClient = commandClient;
			this.logger = logger;
		}

		public override async Task Handle(PingCommand command, string queue) {
			if(command.Round == 0) {
				logger.LogInformation("I won");
				return;
			} else {
				logger.LogInformation($"round :{command.Round}");
				await commandClient.Submit(new PongCommand(command.Round - 1), true);
			}
		}
	}
}
