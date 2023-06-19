using Albatross.Messaging.Services;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing {
	public interface ISubscriptionClient {
		Task<Subscription> Subscribe(ISubscriber subscriber, string topic);
		Subscription Unsubscribe(ISubscriber subscriber, string topic);
	}

	public class SubscriptionClient : ISubscriptionClient {
		private readonly DealerClient dealerClient;
		private readonly SubscriptionService service;

		public SubscriptionClient(DealerClient dealerClient, SubscriptionService service) {
			this.dealerClient = dealerClient;
			this.service = service;
		}
		public Task<Subscription> Subscribe(ISubscriber subscriber, string topic)
			=> this.service.Subscribe(dealerClient, subscriber, topic);

		public Subscription Unsubscribe(ISubscriber subscriber, string topic)
			=> this.service.Unsubscribe(dealerClient, subscriber, topic);
	}
}
