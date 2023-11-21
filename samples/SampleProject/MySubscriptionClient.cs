using Albatross.Messaging.Commands;
using Albatross.Messaging.PubSub.Sub;

namespace SampleProject  {
	public interface IMySubscriptionClient : ISubscriptionClient { }
	public class MySubscriptionClient : SubscriptionClient, IMySubscriptionClient {
		public MySubscriptionClient(MyDealerClientBuilder builder) : base(builder.DealerClient, builder.SubscriptionService) {
		}
	}
	public interface IMyCommandClient : ICommandClient { 
	}
	public class MyCommandClient : CallbackCommandClient, IMyCommandClient {
		public MyCommandClient(MyDealerClientBuilder builder) : base(builder.DealerClient, builder.CommandClientService) {
		}
		public override void OnCommandErrorCallback(ulong id, string commandType, string errorType, byte[] message) {
			throw new System.NotImplementedException();
		}
		public override void OnCommandCallback(ulong id, string commandType, byte[] message) {
			throw new System.NotImplementedException();
		}
	}
}
