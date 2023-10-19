using Albatross.Messaging.Commands;

namespace SampleProject.Commands {
	[Command]
	public class DoNothingCommand {
		public int Id { get; set; }

		public DoNothingCommand(int id) {
			this.Id = id;
		}
	}
}
