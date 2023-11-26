using Albatross.Messaging.Services;

namespace Albatross.Messaging.PubSub.Sub {
	public class SubscriptionCallback : MessageCallback {
		public ISubscriber Subscriber { get; init; }
		public SubscriptionCallback(ISubscriber subscriber){
			this.Subscriber = subscriber;
		}
	}
}
