using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Albatross.Commands {
	public static class Extensions {

		public const string DefaultQueueName = "Default";
		public static IServiceCollection AddCommandBus(this IServiceCollection services) {
			services.TryAddSingleton<ICommandBus, CommandBus>();
			return services;
		}

		static string GetDefaultQueueName(IServiceProvider provider, Command cmd) => DefaultQueueName;

		public static IServiceCollection AddCommand<T>(this IServiceCollection services, Func<IServiceProvider, T, string>? getQueueName = null) where T:Command{
			services.TryAddSingleton<IRegisterCommand>(provider => new RegisterCommand<T, CommandQueue>(provider, getQueueName ?? GetDefaultQueueName));
			services.TryAddTransient<CommandQueue>();
			return services;
		}

		public static IServiceCollection AddCommand<T, Q>(this IServiceCollection services, Func<IServiceProvider, Command, string>? getQueueName = null) 
			where T : Command 
			where Q: class, ICommandQueue {
			services.TryAddSingleton<IRegisterCommand>(provider => new RegisterCommand<T, Q>(provider, getQueueName ?? GetDefaultQueueName));
			services.TryAddTransient<Q>();
			return services;
		}

		public static IServiceCollection AddCommandHandler<H>(this IServiceCollection services) {
			if (typeof(H).TryGetClosedGenericType(typeof(ICommandHandler<>), out Type genericType)) {
				services.TryAddScoped(genericType, typeof(H));
			} else {
				throw new ArgumentException($"{typeof(H).FullName} is not a valid command handler type");
			}
			return services;
		}

		public static IServiceCollection AddDefaultEventPublisher<T>(this IServiceCollection services) {
			services.TryAddSingleton<IEventPublisher<T>, DefaultEventPublisher<T>>();
			return services;
		}
	}
}
