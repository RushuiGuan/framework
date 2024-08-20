using Albatross.Messaging.PubSub.Sub;

namespace Sample.Commands {
	public interface IMySubscriptionClient : ISubscriptionClient { }
	public class MySubscriptionClient : SubscriptionClient, IMySubscriptionClient {
		public MySubscriptionClient(MyDealerClientBuilder builder) : base(builder.DealerClient, builder.SubscriptionService) {
		}
	}
}
