namespace Sample.Commands {
	
	public class PingCommand {
		public PingCommand(int round) {
			this.Round = round;
		}

		public int Round { get; set; }
	}
	
	public class PongCommand  {
		public PongCommand(int round) {
			this.Round = round;
		}

		public int Round { get; set; }
	}
}
