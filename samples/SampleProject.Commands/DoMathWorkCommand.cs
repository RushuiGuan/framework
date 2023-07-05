using Albatross.Messaging.Commands;

namespace SampleProject.Commands {
	[Command(typeof(long))]
	public class DoMathWorkCommand {
		public long Counter { get; init; }

		public DoMathWorkCommand(long counter) {
			this.Counter = counter;
		}
	}
}
