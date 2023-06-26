using Albatross.Messaging.Commands;
using Albatross.Messaging.Eventing;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SampleProject {
	public static class Extension {

		public static IServiceCollection AddSampleProjectCommands(this IServiceCollection services) {
			return services.AddCommand<DoMathWorkCommand, long>((_, provider) => "math-queue")
					.AddCommand<ProcessDataCommand, long>((_, provider) => "math-queue")
					.AddCommand<LongRunningCommand, int>((_, provider) => "show-running-queue")
					.AddCommand<UnstableCommand, int>((_, provider) => "slow-running-queue")
					.AddCommand<PublishCommand>()
					.AddCommand<FireAndForgetCommand>((_, provider) => "fire-and-forget-queue")
					// use the same queue intentionally for this test
					.AddCommand<PingCommand>((_, args) => "ping pong")
					.AddCommand<PongCommand>((_, args) => "ping pong");
		}

		public static IServiceCollection AddSampleProjectClient(this IServiceCollection services) {
			services.AddSampleProjectCommands()
				.AddCommandClient()
				.AddSubscriber()
				.AddDefaultDealerClientConfig();
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