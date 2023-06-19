
namespace Albatross.Messaging.Eventing {
	public record class Subscription {
		public bool On { get; init; }
		public string Topic { get; init; }
		public ISubscriber Subscriber { get; init; }

		public Subscription(string topic, ISubscriber subscriber, bool on) {
			Topic = topic;
			this.Subscriber = subscriber;
			this.On = on;
		}
	}
}
