namespace Sample.Commands {
	
	public class PublishCommand {
		public PublishCommand(string topic, int min, int max) {
			Topic = topic;
			Min = min;
			Max = max;
		}

		public string Topic { get; }
		public int Min { get; }
		public int Max { get; }
	}
}