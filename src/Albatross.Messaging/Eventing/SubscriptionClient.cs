using Albatross.Messaging.Services;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing {
	public interface ISubscriptionClient {
		Task<Subscription> Subscribe(ISubscriber subscriber, params string[] topics);
		Subscription Unsubscribe(ISubscriber subscriber, params string[] topics);
	}

	public class SubscriptionClient : ISubscriptionClient {
		private readonly DealerClient dealerClient;
		private readonly SubscriptionService service;

		public SubscriptionClient(DealerClient dealerClient, SubscriptionService service) {
			this.dealerClient = dealerClient;
			this.service = service;
		}
		public Task<Subscription> Subscribe(ISubscriber subscriber, params string[] topics)
			=> this.service.Subscribe(dealerClient, subscriber, topics);

		public Subscription Unsubscribe(ISubscriber subscriber, params string[] topics)
			=> this.service.Unsubscribe(dealerClient, subscriber, topics);
	}
}
