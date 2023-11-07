using Albatross.Messaging.Eventing.Pub;
using Albatross.Messaging.Eventing.Sub;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Albatross.Messaging.Eventing {
	public static class Extensions {
		public static IServiceCollection AddPublisher(this IServiceCollection services) {
			services.TryAddSingleton<IPublisher, Publisher>();
			services.TryAddSingleton<IPublisherService, PublisherService>();
			services.AddSingleton<IRouterServerService, PublisherReplayService>();
			services.AddSingleton<IRouterServerService>(args=>args.GetRequiredService<IPublisherService>());
			services.TryAddSingleton<ISubscriptionManagement, SubscriptionManagement>();
			services.AddRouterServer();
			return services;
		}

		public static IServiceCollection AddSubscriber(this IServiceCollection services) {
			services.TryAddSingleton<SubscriptionService>();
			services.AddSingleton<IDealerClientService>(args=>args.GetRequiredService<SubscriptionService>());
			services.TryAddSingleton<ISubscriptionClient, SubscriptionClient>();
			services.AddDealerClient();
			return services;
		}
	}
}
