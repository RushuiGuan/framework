using Albatross.Messaging.Commands;
using Albatross.Messaging.Eventing;
using Microsoft.Extensions.DependencyInjection;

namespace SampleProject {
	public static class Extension {

		public static IServiceCollection AddSampleProjectCommands(this IServiceCollection services) {
			return services.AddCommand<DoMathWorkCommand>((_, provider) => "math-queue")
					.AddCommand<ProcessDataCommand>((_, provider) => "math-queue")
					.AddCommand<LongRunningCommand>((_, provider) => "show-running-queue")
					.AddCommand<UnstableCommand>((_, provider) => "slow-running-queue")
					.AddCommand<PublishCommand>()
					.AddCommand<FireAndForgetCommand>((_, provider) => "fire-and-forget-queue")
					// use the same queue intentionally for this test
					.AddCommand<PingCommand>((_, args) => "ping pong")
					.AddCommand<PongCommand>((_, args) => "ping pong");
		}

		public static IServiceCollection AddSampleProjectClient(this IServiceCollection services) {
			services.AddSampleProjectCommands()
				.AddCommandClient()
				.AddSubscriber();
			return services;
		}

		public static IServiceCollection AddSampleProjectCommandBus(this IServiceCollection services) {
			services.AddSampleProjectCommands()
				.AddCommandHandler<DoMathWorkCommandHandler>()
				.AddCommandHandler<ProcessDataCommandHandler>()
				.AddCommandHandler<LongRunningCommandHandler>()
				.AddCommandHandler<PublishCommandHandler>()
				.AddCommandHandler<FireAndForgetCommandHandler>()
				.AddCommandHandler<UnstableCommandHandler>()
				.AddCommandHandler<PingCommandHandler>()
				.AddCommandHandler<PongCommandHandler>()
				.AddCommandBus()
				.AddPublisher();
			return services;
		}
	}
}