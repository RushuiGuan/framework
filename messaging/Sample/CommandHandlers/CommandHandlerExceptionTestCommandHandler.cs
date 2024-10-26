using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Sample.CommandHandlers {
	public class CommandHandlerExceptionTestCommandHandler : BaseCommandHandler<CommandHandlerExceptionTestCommand> {
		private readonly CommandContext context;
		private readonly ICommandClient client;
		private readonly ILogger<CommandHandlerExceptionTestCommandHandler> logger;

		public CommandHandlerExceptionTestCommandHandler(CommandContext context, InternalCommandClient commandClient, ILogger<CommandHandlerExceptionTestCommandHandler> logger) {
			this.context = context;
			client = commandClient;
			this.logger = logger;
		}

		public override async Task Handle(CommandHandlerExceptionTestCommand command) {
			if (command.Delay > 0) {
				await Task.Delay(command.Delay);
			}
			throw new InvalidOperationException("This is a test exception");
		}
	}
}