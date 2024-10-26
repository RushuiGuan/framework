using Albatross.Messaging.Services;
using Albatross.Threading;
using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.PubSub.Sub {
	public interface ISubscriptionClient {
		Task Subscribe(ISubscriber subscriber, string pattern, int timeout = 2000);
		Task Unsubscribe(ISubscriber subscriber, string pattern, int timeout = 2000);
		Task UnsubscribeAll(int timeout = 2000);
	}

	public class SubscriptionClient : ISubscriptionClient {
		private readonly IMessagingService dealerClient;
		private readonly SubscriptionService service;

		public SubscriptionClient(DealerClient dealerClient, SubscriptionService service) {
			this.dealerClient = dealerClient;
			this.service = service;
		}
		public Task Subscribe(ISubscriber subscriber, string pattern, int timeout)
			=> service.Subscribe(dealerClient, subscriber, pattern).WithTimeOut(TimeSpan.FromMilliseconds(timeout));

		public Task Unsubscribe(ISubscriber subscriber, string pattern, int timeout)
			=> service.Unsubscribe(dealerClient, subscriber, pattern).WithTimeOut(TimeSpan.FromMilliseconds(timeout));

		public Task UnsubscribeAll(int timeout)
			=> service.UnsubscribeAll(dealerClient).WithTimeOut(TimeSpan.FromMilliseconds(timeout));
	}
}