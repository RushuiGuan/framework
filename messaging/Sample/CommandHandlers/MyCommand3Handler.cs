using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Sample.CommandHandlers {
	public class MyCommand3Handler : BaseCommandHandler<MyCommand3, MyResult> {
		private readonly CommandContext context;
		private readonly ICommandClient client;
		private readonly ILogger<MyCommand3Handler> logger;

		public MyCommand3Handler(CommandContext context, InternalCommandClient commandClient, ILogger<MyCommand3Handler> logger) {
			this.context = context;
			client = commandClient;
			this.logger = logger;
		}

		public override async Task<MyResult> Handle(MyCommand3 command) {
			logger.LogInformation("Running: {cmd}", command.Name);
			logger.LogInformation("Context: {context}", context);
			if (command.Delay > 0) {
				await Task.Delay(command.Delay);
			}
			await client.SubmitCollection(command.Commands);
			if (command.Error) {
				throw new InvalidOperationException($"Error executing command {command.Name}");
			}
			return new MyResult(command.Name);
		}
	}
}
