namespace Albatross.Messaging.Commands {
	public class CommandQueueInfo {
		public string Name { get; init; }
		public int Count { get; init; }

		public CommandQueueInfo(string name, int count) {
			this.Name = name;
			this.Count = count;
		}
	}
}