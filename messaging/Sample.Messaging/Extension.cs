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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Albatross.Messaging.PubSub.Sub;

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





		public static IServiceCollection AddMessagingDaemonServices(this IServiceCollection services) {
			services.AddCommandBus()
				.AddAssemblyCommandHandlers(typeof(Sample.Messaging.Extensions).Assembly, GetQueueName)
				.AddPublisher();
			return services;
		}

	}
}