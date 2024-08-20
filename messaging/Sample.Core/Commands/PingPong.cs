namespace Sample.Core.Commands {

	public class PingCommand {
		public PingCommand(int round) {
			Round = round;
		}

		public int Round { get; set; }
	}

	public class PongCommand {
		public PongCommand(int round) {
			Round = round;
		}

		public int Round { get; set; }
	}
}
