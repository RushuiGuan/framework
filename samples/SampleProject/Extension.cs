using Albatross.Messaging.Commands;
using Albatross.Messaging.Eventing;
using Albatross.Messaging.Eventing.Sub;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace SampleProject {
	public static class Extensions {
		static string GetQueueName(object command, IServiceProvider provider) {
			switch (command) {
				case DoMathWorkCommand:
				case ProcessDataCommand:
					return "math-queue";
				case LongRunningCommand:
				case UnstableCommand:
					return "show-running-queue";
				case FireAndForgetCommand:
					return "fire-and-forget-queue";
				case PingCommand:
				case PongCommand:
					return "ping pong";
			}
			return Albatross.Messaging.Commands.Extensions.DefaultQueueName;
		}
		public static IServiceCollection AddSampleProjectCommands(this IServiceCollection services) 
			=> services.AddAssemblyCommands(typeof(SampleProject.Extensions).Assembly, GetQueueName);

		public static IServiceCollection AddSampleProjectClient(this IServiceCollection services) {
			services.AddSampleProjectCommands()
				.AddCommandClient()
				.AddSubscriber()
				.AddDefaultDealerClientConfig();

			services.TryAddSingleton<MySubscriber>();
			return services;
		}

		public static void UseSampleProjectClient(this IServiceProvider provider) {
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
	}
}