using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Commands;
using Albatross.Messaging.PubSub;
using Albatross.Messaging.PubSub.Sub;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Albatross.Config;

namespace Sample.Messaging.Commands {
	public static class Extensions {
		public static IServiceCollection AddCustomMessagingClient(this IServiceCollection services) {
			services.TryAddSingleton<IMessageFactory, MessageFactory>();
			services.AddConfig<MessagingConfiguration>();
			services.TryAddSingleton<ICommandClient, MyCommandClient>();
			services.TryAddSingleton(args => new MyDealerClientBuilder(args, args.GetRequiredService<MessagingConfiguration>().DealerClient));
			// Use a custom ISubscriptionClient if need to connect to multiplie subscription endpoint
			services.TryAddSingleton<IMySubscriptionClient, MySubscriptionClient>();
			services.TryAddSingleton<MySubscriber>();
			services.TryAddSingleton<ISubscriber>(x => x.GetRequiredService<MySubscriber>());
			return services;
		}
		public static IServiceCollection AddDefaultMessagingClient(this IServiceCollection services) {
			services.AddCommandClient()
				.AddSubscriber()
				.AddDefaultDealerClientConfig();
			services.TryAddSingleton<MySubscriber>();
			return services;
		}
		public static async Task UseDefaultMessagingClient(this IServiceProvider provider, ILogger logger) {
			provider.GetRequiredService<DealerClient>().Start();
			var subscriber = provider.GetRequiredService<MySubscriber>();
			var client = provider.GetRequiredService<ISubscriptionClient>();
			try {
				await client.Subscribe(subscriber, "^default$");
			} catch (TimeoutException) {
				logger.LogError("Timeout subscribing to sample-messaging-daemon, please make sure that it is running");
			}
		}
		public static async Task UseCustomMessagingClient(this IServiceProvider provider, ILogger logger) {
			provider.GetRequiredService<MyDealerClientBuilder>().DealerClient.Start();
			var subscriber = provider.GetRequiredService<MySubscriber>();
			var client = provider.GetRequiredService<IMySubscriptionClient>();
			try {
				await client.Subscribe(subscriber, "^default$");
			} catch (TimeoutException) {
				logger.LogError("Timeout subscribing to sample-messaging-daemon, please make sure that it is running");
			}
		}
	}
}