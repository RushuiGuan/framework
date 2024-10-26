namespace Albatross.Messaging.PubSub.Sub {
	public record class Subscription {
		public bool On { get; init; }
		public string Pattern { get; init; }
		public ISubscriber Subscriber { get; init; }

		public Subscription(string pattern, ISubscriber subscriber, bool on) {
			Pattern = pattern;
			Subscriber = subscriber;
			On = on;
		}
	}
}