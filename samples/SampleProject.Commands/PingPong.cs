using Albatross.Messaging.Commands;

namespace SampleProject.Commands {
	[Command]
	public class PingCommand {
		public PingCommand(int round) {
			this.Round = round;
		}

		public int Round { get; set; }
	}
	[Command]
	public class PongCommand  {
		public PongCommand(int round) {
			this.Round = round;
		}

		public int Round { get; set; }
	}
}
