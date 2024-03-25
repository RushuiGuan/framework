using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Messaging.Commands;
using System;
using System.Threading.Tasks;

namespace Sample.Messaging {
	public class MyCommand1Handler : BaseCommandHandler<MyCommand1> {
		private readonly CommandContext context;
		private readonly ICommandClient client;
		private readonly ILogger<MyCommand1Handler> logger;

		public MyCommand1Handler(CommandContext context, InternalCommandClient commandClient, ILogger<MyCommand1Handler> logger) {
			this.context = context;
			this.client = commandClient;
			this.logger = logger;
		}

		public override async Task Handle(MyCommand1 command) {
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
