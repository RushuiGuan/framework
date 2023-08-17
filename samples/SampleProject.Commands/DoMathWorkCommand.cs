using Albatross.Messaging;
using Albatross.Messaging.Commands;

namespace SampleProject.Commands {
	[Command(typeof(long))]
	public class DoMathWorkCommand : ILogDetail {
		public long Counter { get; init; }

		public DoMathWorkCommand(long counter) {
			this.Counter = counter;
		}
	}
}
