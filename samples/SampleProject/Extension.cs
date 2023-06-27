using Albatross.Messaging.Commands;
using Albatross.Messaging.Eventing;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
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
			return services;
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