using Albatross.Messaging.Services;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing.Sub {
	public interface ISubscriptionClient {
		Task Subscribe(ISubscriber subscriber, string pattern);
		Task Unsubscribe(ISubscriber subscriber, string pattern);
		Task UnsubscribeAll();
	}

	public class SubscriptionClient : ISubscriptionClient {
		private readonly DealerClient dealerClient;
		private readonly SubscriptionService service;

		public SubscriptionClient(DealerClient dealerClient, SubscriptionService service) {
			this.dealerClient = dealerClient;
			this.service = service;
		}
		public Task Subscribe(ISubscriber subscriber, string pattern)
			=> service.Subscribe(dealerClient, subscriber, pattern);

		public Task Unsubscribe(ISubscriber subscriber, string pattern)
			=> service.Unsubscribe(dealerClient, subscriber, pattern);

		public Task UnsubscribeAll() => service.UnsubscribeAll(dealerClient);
	}
}
