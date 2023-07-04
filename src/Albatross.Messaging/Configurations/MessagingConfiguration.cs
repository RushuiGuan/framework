using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.Messaging.Configurations {
	public class MessagingConfiguration : ConfigBase {

		public override string Key => "messaging";
		public MessagingConfiguration(IConfiguration configuration) : base(configuration) {
			string endpoint = configuration.GetRequiredEndPoint("messaging-server", false);
			this.RouterServer.EndPoint = endpoint;
			this.DealerClient.EndPoint = endpoint;
		}

		public RouterServerConfiguration RouterServer { get; set; } = new RouterServerConfiguration();
		public DealerClientConfiguration DealerClient { get; set; } = new DealerClientConfiguration();
	}
}