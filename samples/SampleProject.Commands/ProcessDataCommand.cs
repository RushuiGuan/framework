using Albatross.Messaging.Commands;

namespace SampleProject.Commands {
	[Command(typeof(long))]
	public class ProcessDataCommand {
		public long Counter { get; init; }

		public ProcessDataCommand(long counter) {
			this.Counter = counter;
		}
	}
}
