using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Messaging.Commands;
using System.Threading.Tasks;

namespace Sample.Messaging {
	public class BadCommandHandler : BaseCommandHandler<MyBadCommand> {
		private readonly ILogger logger;

		public BadCommandHandler(ILogger logger) {
			this.logger = logger;
		}

		public override Task Handle(MyBadCommand command) {
			logger.LogInformation("I should not be reached");
			return Task.CompletedTask;
		}
	}
}
