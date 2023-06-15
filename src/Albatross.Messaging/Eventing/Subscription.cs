using System.Collections.Generic;

namespace Albatross.Messaging.Eventing {
	public record class Subscription {
		public bool On { get; init; }
		public IReadOnlyCollection<string> Topics { get; init; }
		public ISubscriber Subscriber { get; init; }

		public Subscription(IEnumerable<string> topics, ISubscriber subscriber, bool on) {
			Topics = new List<string>(topics);
			this.Subscriber = subscriber;
			this.On = on;
		}
	}
}
