using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject {
	public class KickOffDoNothingCommandHandler : BaseCommandHandler<KickOffDoNothingCommand> {
		private readonly InternalCommandClient client;
		private readonly ILogger<KickOffDoNothingCommandHandler> logger;

		public KickOffDoNothingCommandHandler(InternalCommandClient client, ILogger<KickOffDoNothingCommandHandler> logger) {
			this.client = client;
			this.logger = logger;
		}

		public override Task Handle(KickOffDoNothingCommand command) {
			for(int i=0; i<1000; i++) {
				var id = client.Submit(new DoNothingCommand(i));
				logger.LogInformation("Kicked off donothing command: {id}", id);
			}
			return Task.CompletedTask;
		}
	}
}
