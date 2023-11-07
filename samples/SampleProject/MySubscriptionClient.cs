using Albatross.Messaging.Commands;
using Albatross.Messaging.PubSub.Sub;

namespace SampleProject  {
	public interface IMySubscriptionClient : ISubscriptionClient { }
	public class MySubscriptionClient : SubscriptionClient, IMySubscriptionClient {
		public MySubscriptionClient(MyDealerClientBuilder builder) : base(builder.DealerClient, builder.SubscriptionService) {
		}
	}
	public interface IMyCommandClient : ICommandClient { }
	public class MyCommandClient : CommandClient, IMyCommandClient {
		public MyCommandClient(MyDealerClientBuilder builder) : base(builder.DealerClient, builder.CommandClientService) {
		}
	}
}
