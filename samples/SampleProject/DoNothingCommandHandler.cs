using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject {
	public class DoNothingCommandHandler : BaseCommandHandler<DoNothingCommand> {
		private readonly ILogger<DoNothingCommandHandler> logger;

		public DoNothingCommandHandler(ILogger<DoNothingCommandHandler> logger) {
			this.logger = logger;
		}

		public override async Task Handle(DoNothingCommand command, string queue) {
			await Task.Delay(1000);
			logger.LogInformation("{id}, I am done", command.Id);
		}
	}
}
