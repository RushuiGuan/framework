using Albatross.Config;
using Albatross.Messaging.Commands;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Albatross.Messaging.PubSub;
using Albatross.Messaging.PubSub.Sub;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using Sample.PubSub;
using System;
using System.Threading.Tasks;

namespace Sample {
	public static class Extensions {
		public static IServiceCollection AddCustomMessagingClient(this IServiceCollection services) {
			services.TryAddSingleton<IMessageFactory, MessageFactory>();
			services.AddConfig<MessagingConfiguration>();
			services.TryAddSingleton<IMyCommandClient, MyCommandClient>();
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
		public static string GetQueueName(ulong messageId, object command, IServiceProvider provider) {
			switch (command) {
				case SelfDestructCommand:
				case MyCommand1:
					return "my-command-queue1";
				case TestOperationWithResultCommand:
					return $"my-command-queue2_{messageId % 5}";
				case PingCommand:
				case PongCommand:
					return "ping pong";
			}
			return Albatross.Messaging.Commands.Extensions.DefaultQueueName;
		}
	}
}