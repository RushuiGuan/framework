using Albatross.Messaging.Services;
using Albatross.Threading;

namespace Albatross.Messaging.PubSub.Sub {
	public class SubscriptionCallback : TaskCallback<ulong> {
		public ISubscriber Subscriber { get; init; }
		public SubscriptionCallback(ISubscriber subscriber) {
			this.Subscriber = subscriber;
		}
	}
}