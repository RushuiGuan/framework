using Albatross.Config;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Albatross.Messaging.Services {
	public static class Extensions {
		public static void UseDealerClient(this IServiceProvider serviceProvider) {
			var client = serviceProvider.GetRequiredService<DealerClient>();
			client.Start();
		}
		public static void UseRouterServer(this IServiceProvider serviceProvider) {
			var server = serviceProvider.GetRequiredService<RouterServer>();
			server.Start();
		}
		public static IServiceCollection AddDefaultDealerClientConfig(this IServiceCollection services) {
			services.AddConfig<MessagingConfiguration>();
			services.TryAddSingleton(provider => {
				var config = provider.GetRequiredService<MessagingConfiguration>();
				return config.DealerClient;
			});
			return services;
		}
		public static IServiceCollection AddDealerClient(this IServiceCollection services) {
			services.TryAddSingleton<DealerClient>();
			services.TryAddSingleton<IMessageFactory, MessageFactory>();
			return services;
		}
		public static IServiceCollection AddRouterServer(this IServiceCollection services) {
			services.AddConfig<MessagingConfiguration>();
			services.TryAddSingleton(provider => {
				var config = provider.GetRequiredService<MessagingConfiguration>();
				return config.RouterServer;
			});
			services.TryAddSingleton<RouterServer>();
			services.TryAddSingleton<IMessageFactory, MessageFactory>();
			return services;
		}
	}
}