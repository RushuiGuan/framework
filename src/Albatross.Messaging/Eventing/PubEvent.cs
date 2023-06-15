namespace Albatross.Messaging.Eventing {
	public class PubEvent {
		public string Topic { get; set; }
		public byte[] Payload { get; set; }
		public PubEvent(string topic, byte[] payload) {
			this.Topic = topic;
			this.Payload = payload;
		}
	}
}
