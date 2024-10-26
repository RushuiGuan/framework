using Albatross.Messaging.Configurations;
using Albatross.Messaging.Services;
using System;

namespace Sample {
	public class MyDealerClientBuilder : DealerClientBuilder {
		public MyDealerClientBuilder(IServiceProvider serviceProvider, DealerClientConfiguration configuration) : base(serviceProvider, configuration) {
			TryAddCommandClientService();
			TryAddSubscriptionService();
			Build();
		}
	}
}