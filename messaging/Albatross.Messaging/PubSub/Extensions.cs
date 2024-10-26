using Albatross.Messaging.Configurations;
using Albatross.Messaging.PubSub.Pub;
using Albatross.Messaging.PubSub.Sub;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Albatross.Messaging.PubSub {
	public static class Extensions {
		public static IServiceCollection AddPublisher(this IServiceCollection services) {
			services.TryAddSingleton<IPublisher, Publisher>();
			services.TryAddSingleton<IPublisherService, PublisherService>();
			services.AddSingleton<IRouterServerService, PublisherReplayService>();
			services.AddSingleton<IRouterServerService>(args => args.GetRequiredService<IPublisherService>());
			services.TryAddSingleton(args => args.GetRequiredService<MessagingConfiguration>().SubscriptionManagement);
			services.TryAddSingleton<ISubscriptionManagement, SubscriptionManagement>();
			services.AddRouterServer();
			return services;
		}

		public static IServiceCollection AddSubscriber(this IServiceCollection services) {
			services.TryAddSingleton<SubscriptionService>();
			services.AddSingleton<IDealerClientService>(args => args.GetRequiredService<SubscriptionService>());
			services.TryAddSingleton<ISubscriptionClient, SubscriptionClient>();
			services.AddDealerClient();
			return services;
		}
	}
}