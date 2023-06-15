using Albatross.Config;
using Albatross.Reflection;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Albatross.Messaging.DataLogging;

namespace Albatross.Messaging.Commands {
	public static class Extensions {
		static string GetDefaultQueueName<CommandType>(CommandType _, IServiceProvider provider) where CommandType : ICommand => "default_queue";

		public static IServiceCollection AddCommand<T>(this IServiceCollection services, Func<T, IServiceProvider, string>? getQueueName = null) where T : ICommand {
			services.AddSingleton<IRegisterCommand>(provider => new RegisterCommand<T>(getQueueName ?? GetDefaultQueueName));
			return services;
		}

		public static IServiceCollection AddCommandClient(this IServiceCollection services) {
			services.AddConfig<MessagingConfiguration>();
			services.TryAddSingleton(provider => {
				var config = provider.GetRequiredService<MessagingConfiguration>();
				return config.DealerClient;
			});
			services.TryAddSingleton(provider => {
				var config = provider.GetRequiredService<MessagingConfiguration>();
				return config.DealerClient.DiskStorage;
			});
			services.TryAddSingleton<DealerClient>();
			services.TryAddSingleton<ICommandClientService, CommandClient>();
			services.AddSingleton<IDealerClientService>(provider => provider.GetRequiredService<ICommandClientService>());
			services.TryAddSingleton<IMessageFactory, MessageFactory>();
			services.TryAddSingleton<MessagingJsonSerializationOption>();
			services.AddDiskStorageDataLogging();
			return services;
		}

		public static void UseDealerClient(this IServiceProvider serviceProvider) {
			var client = serviceProvider.GetRequiredService<DealerClient>();
			client.Start();
		}
		public static IServiceCollection AddCommandHandler<H>(this IServiceCollection services) {
			if (typeof(H).TryGetClosedGenericType(typeof(ICommandHandler<,>), out Type? genericType)) {
				services.TryAddScoped(genericType, typeof(H));
			} else if (typeof(H).TryGetClosedGenericType(typeof(ICommandHandler<>), out genericType)) {
				services.TryAddScoped(genericType, typeof(H));
			} else {
				throw new ArgumentException($"{typeof(H).FullName} is not a valid command handler type");
			}
			return services;
		}

		public static IServiceCollection AddCommandBus(this IServiceCollection services) {
			services.AddConfig<MessagingConfiguration>();
			services.TryAddSingleton(provider => {
				var config = provider.GetRequiredService<MessagingConfiguration>();
				return config.RouterServer;
			});
			services.TryAddSingleton(provider => {
				var config = provider.GetRequiredService<RouterServerConfiguration>();
				return config.DiskStorage;
			});
			services.TryAddSingleton<RouterServer>();
			services.TryAddSingleton<ICommandBus, CommandBusService>();
			services.AddSingleton<IRouterServerService>(provider => provider.GetRequiredService<ICommandBus>());
			services.AddSingleton<IRouterServerService, CommandReplayService>();
			services.TryAddSingleton<IMessageFactory, MessageFactory>();
			services.TryAddSingleton<MessagingJsonSerializationOption>();
			services.TryAddSingleton<ICommandQueueFactory, CommandQueueFactory>();
			services.TryAddTransient<CommandQueue, TaskCommandQueue>();
			services.TryAddSingleton<CommandReplayService>();
			services.AddDiskStorageDataLogging();
			return services;
		}
	}
}