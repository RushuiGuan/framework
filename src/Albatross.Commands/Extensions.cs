using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Commands {
	public static class Extensions {
		public const string DefaultQueueName = "Default";
		public static IServiceCollection AddCommandBus(this IServiceCollection services) {
			services.AddSingleton<ICommandBus, CommandBus>();
			return services;
		}
		public static IServiceCollection AddCommand<T>(this IServiceCollection services, Func<Command, string>? getQueueName = null) where T:Command{
			services.AddSingleton<IRegisterCommand, RegisterCommand<T, CommandQueue>>(provider => new RegisterCommand<T, CommandQueue>(getQueueName ?? (_=>DefaultQueueName)));
			return services;
		}

		public static IServiceCollection AddCommandHandler<H>(this IServiceCollection services) {
			if (typeof(H).TryGetClosedGenericType(typeof(ICommandHandler<>), out Type genericType)) {
				services.AddSingleton(genericType, typeof(H));
			} else {
				throw new ArgumentException($"{typeof(H).FullName} is not a valid command handler type");
			}
			return services;
		}

		public static void UseCommandBus(this IServiceProvider serviceProvider) {
			serviceProvider.GetRequiredService<ICommandBus>().Start();
		}

		public static IServiceCollection AddDefaultEventPublisher<T>(this IServiceCollection services) {
			services.AddSingleton<IEventPublisher<T>, DefaultEventPublisher<T>>();
			return services;
		}
	}
}
