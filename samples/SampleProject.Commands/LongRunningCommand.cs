using Albatross.Messaging.Commands;

namespace SampleProject.Commands {
	[Command(typeof(int))]
	public class LongRunningCommand { 
		public int Duration { get; init; }
		public int Counter { get; init; }

		public LongRunningCommand(int duration, int counter) {
			Duration = duration;
			Counter = counter;
		}
	}
}
