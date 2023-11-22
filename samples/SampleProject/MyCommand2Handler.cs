using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	public class MyCommand2Handler : BaseCommandHandler<MyCommand2> {
		private readonly CommandContext context;
		private readonly ICommandClient client;
		private readonly ILogger<MyCommand2Handler> logger;

		public MyCommand2Handler(CommandContext context, InternalCommandClient commandClient, ILogger<MyCommand2Handler> logger) {
			this.context = context;
			this.client = commandClient;
			this.logger = logger;
		}

		public override async Task Handle(MyCommand2 command) {
			logger.LogInformation("Running: {cmd}", command.Name);
			logger.LogInformation("Context: {context}", context);
			if (command.Delay > 0) {
				await Task.Delay(command.Delay);
			}
			await client.SubmitCollection(command.Commands);
			if (command.Error) {
				throw new InvalidOperationException($"Error executing command {command.Name}");
			}
		}
	}
}
