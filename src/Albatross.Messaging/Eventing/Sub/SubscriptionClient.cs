using Albatross.Messaging.Services;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing.Sub {
	public interface ISubscriptionClient {
		Task<Subscription> Subscribe(ISubscriber subscriber, string pattern);
		Subscription Unsubscribe(ISubscriber subscriber, string pattern);
	}

	public class SubscriptionClient : ISubscriptionClient {
		private readonly DealerClient dealerClient;
		private readonly SubscriptionService service;

		public SubscriptionClient(DealerClient dealerClient, SubscriptionService service) {
			this.dealerClient = dealerClient;
			this.service = service;
		}
		public Task<Subscription> Subscribe(ISubscriber subscriber, string pattern)
			=> service.Subscribe(dealerClient, subscriber, pattern);

		public Subscription Unsubscribe(ISubscriber subscriber, string pattern)
			=> service.Unsubscribe(dealerClient, subscriber, pattern);
	}
}
