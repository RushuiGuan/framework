using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	public class MyCommandHandler : BaseCommandHandler<MyCommand1> {
		private readonly CommandContext context;
		private readonly ICommandClient client;
		private readonly ILogger<MyCommandHandler> logger;

		public MyCommandHandler(CommandContext context, InternalCommandClient commandClient, ILogger<MyCommandHandler> logger) {
			this.context = context;
			this.client = commandClient;
			this.logger = logger;
		}

		public override async Task Handle(MyCommand1 command) {
			logger.LogInformation("Running MyCommand {cmd} with context {context}", command, context);
			if (command.Delay > 0) {
				await Task.Delay(command.Delay);
			}
			await client.SubmitCollection(command.Commands);
			if (command.Error) {
				throw new InvalidOperationException($"Error executing command {command}");
			}
		}
	}
}
