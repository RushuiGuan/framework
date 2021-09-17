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
		public static ICommandQueue CreateDefaultQueue(string name, IServiceProvider provider)
			=> new CommandQueue(name, provider.GetRequiredService<IServiceScopeFactory>(), provider.GetRequiredService<ILoggerFactory>());

		public static ICommandQueue CreateImprovedQueue(string name, IServiceProvider provider)
			=> new ImprovedCommandQueue(name, provider.GetRequiredService<IServiceScopeFactory>(), provider.GetRequiredService<ILoggerFactory>());

		public static IServiceCollection AddCommand<T>(this IServiceCollection services, Func<T, IServiceProvider, string>? getQueueName = null, Func<string, IServiceProvider, ICommandQueue>? createQueue = null) where T:Command{
			services.AddSingleton<IRegisterCommand>(provider => new RegisterCommand<T>(getQueueName ?? GetDefaultQueueName, createQueue ?? CreateDefaultQueue));
			return services;
		}
		
		public static IServiceCollection AddCommandHandler<H>(this IServiceCollection services) {
			if (typeof(H).TryGetClosedGenericType(typeof(ICommandHandler<,>), out Type genericType)) {
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
