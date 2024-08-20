using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Sample.CommandHandlers {
	public class MyCommand2Handler : BaseCommandHandler<MyCommand2> {
		private readonly CommandContext context;
		private readonly ICommandClient client;
		private readonly ILogger<MyCommand2Handler> logger;

		public MyCommand2Handler(CommandContext context, InternalCommandClient commandClient, ILogger<MyCommand2Handler> logger) {
			this.context = context;
			client = commandClient;
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
