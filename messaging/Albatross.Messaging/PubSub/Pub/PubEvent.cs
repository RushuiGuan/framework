namespace Albatross.Messaging.PubSub.Pub {
	public class PubEvent {
		public string Topic { get; set; }
		public byte[] Payload { get; set; }
		public PubEvent(string topic, byte[] payload) {
			Topic = topic;
			Payload = payload;
		}
	}
}