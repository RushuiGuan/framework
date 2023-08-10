using Albatross.Messaging.Commands;

namespace SampleProject.Commands {
	[Command]
	public class FireAndForgetCommand {
		public int Counter { get; }
		public int? Duration { get; }
		public FireAndForgetCommand(int counter, int? duration) {
			this.Counter = counter;
			Duration = duration;
		}
	}
}
