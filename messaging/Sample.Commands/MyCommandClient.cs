using Albatross.Messaging;
using Albatross.Messaging.Commands;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;

namespace Sample.Commands {
	public class MyCommandClient : CallbackCommandClient {
		private readonly ILogger<MyCommandClient> logger;

		public MyCommandClient(ILogger<MyCommandClient> logger, MyDealerClientBuilder builder) : base(builder.DealerClient, builder.CommandClientService) {
			this.logger = logger;
		}

		public override void OnCommandCallback(ulong id, string commandType, byte[] message) {
			logger.LogInformation("success callback: {id}, {type}, {msg}", id, commandType, message.ToUtf8String());
		}

		public override void OnCommandErrorCallback(ulong id, string commandType, string errorType, byte[] message) {
			logger.LogInformation("failure callback: {id}, {type}, {errorType} {msg}", id, commandType, errorType, message.ToUtf8String());
		}
	}
}
