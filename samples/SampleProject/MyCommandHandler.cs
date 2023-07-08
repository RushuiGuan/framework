﻿using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject {
	public class MyCommandHandler : BaseCommandHandler<MyCommand> {
		private readonly ICommandClient commandClient;
		private readonly ILogger<MyCommandHandler> logger;

		public MyCommandHandler(ICommandClient commandClient, ILogger<MyCommandHandler> logger) {
			this.commandClient = commandClient;
			this.logger = logger;
		}

		public override Task Handle(MyCommand command, string queue) {
			logger.LogInformation("i work ok");
			return Task.CompletedTask;
		}
	}
}
