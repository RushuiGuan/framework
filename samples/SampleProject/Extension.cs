using Albatross.Config;
using SampleProject.Commands;
using Albatross.Messaging.Commands;
using Albatross.Messaging.Eventing;
using Albatross.Messaging.Eventing.Sub;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Albatross.Messaging.Configurations;
using Albatross.Messaging;
using Albatross.Messaging.Messages;

namespace SampleProject {
	public static class Extensions {
		public static IServiceCollection AddDefaultSampleProjectClient(this IServiceCollection services) {
			services.AddSampleProjectCommands()
				.AddCommandClient()
				.AddSubscriber()
				.AddDefaultDealerClientConfig();
			services.TryAddSingleton<MySubscriber>();
			return services;
		}

		public static void UseDefaultSampleProjectClient(this IServiceProvider provider) {
			provider.GetRequiredService<DealerClient>().Start();
			var subscriber = provider.GetRequiredService<MySubscriber>();
			var client = provider.GetRequiredService<ISubscriptionClient>();
			client.Subscribe(subscriber, "^default$");
		}

		public static IServiceCollection AddSampleProjectCommandBus(this IServiceCollection services) {
			services.AddSampleProjectCommands()
				.AddAssemblyCommandHandlers(typeof(SampleProject.Extensions).Assembly)
				.AddCommandBus()
				.AddPublisher();
			return services;
		}

		public static IServiceCollection AddCustomSampleProjectClient(this IServiceCollection services) {
			services.AddConfig<MessagingConfiguration>();
			services.TryAddSingleton<IMessageFactory, MessageFactory>();
			services.TryAddSingleton(args => {
				var builder = new MyDealerClientBuilder(args, args.GetRequiredService<MessagingConfiguration>().DealerClient);
				builder.TryAddCommandClientService().TryAddSubscriptionService().Build();
				return builder;
			});
			services.TryAddSingleton<IMySubscriptionClient, MySubscriptionClient>();
			services.TryAddSingleton<IMyCommandClient, MyCommandClient>();
			services.AddSampleProjectCommands();
			services.TryAddSingleton<MySubscriber>();
			return services;
		}
		public static void UseCustomSampleProjectClient(this IServiceProvider provider) {
			provider.GetRequiredService<MyDealerClientBuilder>().DealerClient.Start();
			var subscriber = provider.GetRequiredService<MySubscriber>();
			var client = provider.GetRequiredService<IMySubscriptionClient>();
			client.Subscribe(subscriber, "^default$");
		}
	}
}