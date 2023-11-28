using Albatross.Config;
using Sample.Messaging.Commands;
using Albatross.Messaging.Commands;
using Albatross.Messaging.PubSub;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;

namespace Sample.Messaging {
	public static class Extensions {
		public static string GetQueueName(object command, IServiceProvider provider) {
			switch (command) {
				case SelfDestructCommand:
				case MyCommand1:
					return "my-command-queue1";
				case MyCommand2:
					return "my-command-queue2";
				case PingCommand:
				case PongCommand:
					return "ping pong";
			}
			return Albatross.Messaging.Commands.Extensions.DefaultQueueName;
		}
		public static IServiceCollection AddDefaultSampleProjectClient(this IServiceCollection services) {
			services.AddCommandClient()
				.AddSubscriber()
				.AddDefaultDealerClientConfig();
			services.TryAddSingleton<MySubscriber>();
			return services;
		}

		public static void UseDefaultSampleProjectClient(this IServiceProvider provider) {
			provider.GetRequiredService<DealerClient>().Start();
			var subscriber = provider.GetRequiredService<MySubscriber>();
			var client = provider.GetRequiredService<IMySubscriptionClient>();
			client.Subscribe(subscriber, "^default$");
		}

		public static IServiceCollection AddSampleProjectDaemon(this IServiceCollection services) {
			services
				.AddAssemblyCommandHandlers(typeof(Sample.Messaging.Extensions).Assembly, GetQueueName)
				.AddCommandBus()
				.AddPublisher();
			return services;
		}

		public static IServiceCollection AddSampleProjectClientApi(this IServiceCollection services) {
			services.AddConfig<MessagingConfiguration>();
			services.TryAddSingleton<IMessageFactory, MessageFactory>();
			services.TryAddSingleton(args => {
				var builder = new MyDealerClientBuilder(args, args.GetRequiredService<MessagingConfiguration>().DealerClient);
				builder.TryAddCommandClientService().TryAddSubscriptionService().Build();
				return builder;
			});
			// Use a custom ISubscriptionClient if need to connect to multiplie subscription endpoint
			services.TryAddSingleton<IMySubscriptionClient, MySubscriptionClient>();
			services.TryAddSingleton<MySubscriber>();
			services.TryAddSingleton<ISubscriber>(x => x.GetRequiredService<MySubscriber>());
			return services;
		}
		public static void UseSampleProjectWebApi(this IServiceProvider provider) {
			provider.GetRequiredService<MyDealerClientBuilder>().DealerClient.Start();
			var subscriber = provider.GetRequiredService<MySubscriber>();
			var client = provider.GetRequiredService<IMySubscriptionClient>();
			client.Subscribe(subscriber, "^default$");
		}
	}
}