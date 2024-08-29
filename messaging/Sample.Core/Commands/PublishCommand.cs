using Sample.Core.Commands.MyOwnNameSpace;

namespace Sample.Core.Commands {

	public class PublishCommand : IApplicationCommand {
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