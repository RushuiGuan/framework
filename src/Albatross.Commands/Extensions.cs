using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Commands {
	public static class Extensions {
		public static IServiceCollection AddCommandBus(this IServiceCollection services) {
			services.TryAddSingleton<ICommandBus, CommandBus>();
			return services;
		}

		static string GetDefaultQueueName(Command _, IServiceProvider provider) => "Default";
		static CommandQueue CreateDefaultQueue(string name, IServiceProvider provider)
			=> new CommandQueue(name, provider.GetRequiredService<IServiceScopeFactory>(), provider.GetRequiredService<ILogger<CommandQueue>>());

		public static IServiceCollection AddCommand<T>(this IServiceCollection services, Func<T, IServiceProvider, string>? getQueueName = null) where T:Command{
			services.AddSingleton<IRegisterCommand>(provider => new RegisterCommand<T, CommandQueue>(getQueueName ?? GetDefaultQueueName, CreateDefaultQueue));
			services.TryAddTransient<CommandQueue>();
			return services;
		}
		
		public static IServiceCollection AddCommand<T, Q>(this IServiceCollection services, Func<Command, IServiceProvider, string>? getQueueName, Func<string, IServiceProvider, Q> createQueue) 
			where T : Command 
			where Q: class, ICommandQueue {
			services.AddSingleton<IRegisterCommand>(provider => new RegisterCommand<T, Q>(getQueueName ?? GetDefaultQueueName, createQueue));
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
