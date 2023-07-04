using Albatross.Messaging.Services;

namespace Albatross.Messaging.Eventing.Sub {
	public class SubscriptionCallback : MessageCallback {
		public ISubscriber Subscriber { get; init; }
		public SubscriptionCallback(ISubscriber subscriber){
			this.Subscriber = subscriber;
		}
	}
}
