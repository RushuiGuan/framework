using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject {
	public class MyCommandHandler : BaseCommandHandler<MyCommand> {
		private readonly ICommandClient commandClient;
		private readonly ILogger<MyCommandHandler> logger;

		public MyCommandHandler(InternalCommandClient commandClient, ILogger<MyCommandHandler> logger) {
			this.commandClient = commandClient;
			this.logger = logger;
		}

		public override Task Handle(MyCommand command) {
			logger.LogInformation("i work ok");
			return Task.CompletedTask;
		}
	}
}
