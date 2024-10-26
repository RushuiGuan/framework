using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using System.Threading.Tasks;

namespace Sample.CommandHandlers {
	public class SerializationErrorTestCommandHandler : BaseCommandHandler<SerializationErrorTestCommand> {
		private readonly ILogger logger;

		public SerializationErrorTestCommandHandler(ILogger logger) {
			this.logger = logger;
		}

		public override Task Handle(SerializationErrorTestCommand command) {
			logger.LogInformation("I should not be reached");
			return Task.CompletedTask;
		}
	}
}