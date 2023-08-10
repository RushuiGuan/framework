namespace Albatross.Messaging.Eventing.Pub {
	public class PubEvent {
		public string Topic { get; set; }
		public byte[] Payload { get; set; }
		public PubEvent(string topic, byte[] payload) {
			Topic = topic;
			Payload = payload;
		}
	}
}
