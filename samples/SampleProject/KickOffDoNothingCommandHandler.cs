using Albatross.Messaging.Commands;
using SampleProject.Commands;
using System.Threading.Tasks;

namespace SampleProject {
	public class KickOffDoNothingCommandHandler : BaseCommandHandler<KickOffDoNothingCommand> {
		private readonly InternalCommandClient client;

		public KickOffDoNothingCommandHandler(InternalCommandClient client) {
			this.client = client;
		}

		public override async Task Handle(KickOffDoNothingCommand command, string queue) {
			for(int i=0; i<1000; i++) {
				await client.Submit(new DoNothingCommand(i));
			}
		}
	}
}
