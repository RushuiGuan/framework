using Albatross.Messaging.Core;
using Sample.Core.Commands.MyOwnNameSpace;

namespace Sample.Core.Commands {
	public class PingCommand : IApplicationCommand {
		public PingCommand(int round) {
			Round = round;
		}

		public int Round { get; set; }
	}

	public class PongCommand : IApplicationCommand {
		public PongCommand(int round) {
			Round = round;
		}

		public int Round { get; set; }
	}
}