using Albatross.Reflection;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

namespace Albatross.Messaging.Commands {
	public static class Extensions {
		public const string DefaultQueueName = "default_queue";

		public static IServiceCollection AddCommand<T>(this IServiceCollection services, Func<T, IServiceProvider, string>? getQueueName = null) where T : notnull {
			services.AddSingleton<IRegisterCommand>(provider => new RegisterCommand<T>(getQueueName ?? ((_, provider) => DefaultQueueName)));
			return services;
		}

		public static IServiceCollection AddCommand<T, K>(this IServiceCollection services, Func<T, IServiceProvider, string>? getQueueName = null) where T : notnull where K : notnull {
			services.AddSingleton<IRegisterCommand>(provider => new RegisterCommand<T, K>(getQueueName ?? ((_, provider) => DefaultQueueName)));
			return services;
		}

		public static IServiceCollection AddAssemblyCommands(this IServiceCollection services, Assembly assembly, Func<object, IServiceProvider, string>? getQueueName = null) {
			foreach(var type in assembly.GetConcreteClasses()) {
				var attrib = type.GetCustomAttribute<CommandAttribute>();
				if (attrib != null) {
					services.AddSingleton<IRegisterCommand>(new RegisterCommand(type, attrib.ResponseType, getQueueName ?? ((_, provider) => DefaultQueueName)));
				}
			}
			return services;
		}

		public static IServiceCollection AddCommandClient(this IServiceCollection services) {
			services.TryAddSingleton<CommandClientService>();
			services.AddSingleton<IDealerClientService>(args => args.GetRequiredService<CommandClientService>());
			services.TryAddSingleton<ICommandClient, CommandClient>();
			services.TryAddSingleton<MessagingJsonSerializationOption>();
			services.AddDealerClient();
			return services;
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
		public static IServiceCollection AddAssemblyCommandHandlers(this IServiceCollection services, Assembly assembly) {
			var types = assembly.GetConcreteClasses<ICommandHandler>();
			foreach (var type in types) {
				if (type.TryGetClosedGenericType(typeof(ICommandHandler<,>), out Type? genericType)) {
					services.TryAddScoped(genericType, type);
				} else if (type.TryGetClosedGenericType(typeof(ICommandHandler<>), out genericType)) {
					services.TryAddScoped(genericType, type);
				}
			}
			return services;
		}

		public static IServiceCollection AddCommandBus(this IServiceCollection services) {
			services.TryAddSingleton<ICommandBusService, CommandBusService>();
			services.AddSingleton<IRouterServerService>(provider => provider.GetRequiredService<ICommandBusService>());
			services.AddSingleton<IRouterServerService, CommandBusReplayService>();
			services.TryAddSingleton<MessagingJsonSerializationOption>();
			services.TryAddSingleton<ICommandQueueFactory, CommandQueueFactory>();
			services.TryAddTransient<CommandQueue, TaskCommandQueue>();
			// this should only be used if the TaskCommandQueue is used
			services.TryAddSingleton<ICommandClient, InternalCommandClient>();
			services.AddRouterServer();
			return services;
		}
	}
}