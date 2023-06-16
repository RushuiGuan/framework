using Albatross.Config;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Albatross.Messaging.DataLogging;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Albatross.Messaging.Eventing {
	public static class Extensions {
		public static IServiceCollection AddPublisher(this IServiceCollection services) {
			services.AddConfig<MessagingConfiguration>();
			services.TryAddSingleton(provider => {
				var config = provider.GetRequiredService<MessagingConfiguration>();
				return config.RouterServer;
			});
			services.TryAddSingleton(provider => {
				var config = provider.GetRequiredService<RouterServerConfiguration>();
				return config.DiskStorage;
			});

			services.TryAddSingleton<RouterServer>();
			services.TryAddSingleton<IPublisher, Publisher>();
			services.AddSingleton<IRouterServerService, PublisherService>();
			services.TryAddSingleton<IMessageFactory, MessageFactory>();
			services.TryAddSingleton<IDataLogWriter, DiskStorageLogWriter>();
			return services;
		}

		public static IServiceCollection AddSubscriber(this IServiceCollection services) {
			services.AddConfig<MessagingConfiguration>();
			services.TryAddSingleton(provider => {
				var config = provider.GetRequiredService<MessagingConfiguration>();
				return config.DealerClient;
			});
			services.TryAddSingleton(provider => {
				var config = provider.GetRequiredService<MessagingConfiguration>();
				return config.DealerClient.DiskStorage;
			});

			services.TryAddSingleton<DealerClient>();
			services.TryAddSingleton<ISubscriptionClient, SubscriptionClient>();
			services.AddSingleton<SubscriptionService>();
			services.AddSingleton<IDealerClientService>(args => args.GetRequiredService<SubscriptionService>());
			services.TryAddSingleton<IMessageFactory, MessageFactory>();
			services.TryAddSingleton<IDataLogWriter, DiskStorageLogWriter>();
			return services;
		}
	}
}
