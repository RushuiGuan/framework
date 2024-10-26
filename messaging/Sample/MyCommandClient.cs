using Albatross.Messaging;
using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;

namespace Sample {
	public interface IMyCommandClient : ITaskCallbackCommandClient, ICommandClient { }
	public class MyCommandClient : TaskCallbackCommandClient, IMyCommandClient {
		private readonly ILogger<MyCommandClient> logger;

		public MyCommandClient(ILogger<MyCommandClient> logger, MyDealerClientBuilder builder)
			: base(builder.DealerClient, builder.CommandClientService) {
			this.logger = logger;
		}
	}
}