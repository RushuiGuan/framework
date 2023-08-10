using Albatross.Messaging.Commands;

namespace SampleProject.Commands {
	[Command(typeof(int))]
	public class UnstableCommand {
		public int Counter { get; }
		public UnstableCommand(int counter) {
			this.Counter = counter;
		}
	}
}