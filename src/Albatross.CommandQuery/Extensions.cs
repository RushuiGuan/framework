using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.CommandQuery {
	public static class Extensions {
		public const string DefaultQueueName = "Default";
		public static IServiceCollection AddCommand<T>(this IServiceCollection services, Func<Command, string>? getQueueName = null) where T:Command{
			services.AddSingleton<IRegisterCommand, RegisterCommand<T, CommandQueue>>(provider => new RegisterCommand<T, CommandQueue>(getQueueName ?? (_=>DefaultQueueName)));
			return services;
		}

		public static IServiceCollection AddCommandHandler<T, H>(this IServiceCollection services) 
			where T : Command 
			where H: class, ICommandHandler<T> {

			services.AddSingleton<ICommandHandler<T>, H>();
			return services;
		}

		public static void UseCommandBus(this IServiceProvider serviceProvider) {
			serviceProvider.GetRequiredService<ICommandBus>().Start();
		}
	}
}
