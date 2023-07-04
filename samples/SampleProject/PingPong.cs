﻿using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SampleProject {
	[Command]
	public class PingCommand {
		public PingCommand(int round) {
			this.Round = round;
		}

		public int Round { get; set; }
	}
	[Command]
	public class PongCommand  {
		public PongCommand(int round) {
			this.Round = round;
		}

		public int Round { get; set; }
	}
	public class PingCommandHandler : BaseCommandHandler<PingCommand> {
		private readonly ICommandClient commandClient;
		private readonly ILogger<PingCommandHandler> logger;

		public PingCommandHandler(ICommandClient commandClient, ILogger<PingCommandHandler> logger) {
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
