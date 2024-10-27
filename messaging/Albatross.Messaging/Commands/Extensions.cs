using Albatross.Messaging.Services;
using Albatross.Reflection;
using Albatross.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	public static class Extensions {
		public const string DefaultQueueName = "default_queue";
		public static string GetDefaultQueueName(object _, IServiceProvider provider) => DefaultQueueName;

		//static IServiceCollection AddCommand(this IServiceCollection services, Type commandType, Type? responseType, Func<object, IServiceProvider, string> getQueueName) {
		//	services.AddSingleton<IRegisterCommand>(new RegisterCommand(commandType, responseType ?? typeof(void), getQueueName));
		//	return services;
		//}
		public static IServiceCollection AddCommand<T>(this IServiceCollection services, Func<T, IServiceProvider, string> getQueueName) where T : notnull {
			services.AddSingleton<IRegisterCommand<T>>(new RegisterCommand<T>(getQueueName));
			return services;
		}
		public static IServiceCollection AddCommand<T, K>(this IServiceCollection services, Func<T, IServiceProvider, string> getQueueName) where T : notnull where K : notnull {
			services.AddSingleton<IRegisterCommand<T, K>>(new RegisterCommand<T, K>(getQueueName));
			return services;
		}

		public static IServiceCollection AddCommandClient<T>(this IServiceCollection services) where T : class, ICommandClient {
			services.TryAddSingleton<CommandClientService>();
			services.AddSingleton<IDealerClientService>(args => args.GetRequiredService<CommandClientService>());
			services.TryAddSingleton<ICommandClient, T>();
			services.AddDealerClient();
			return services;
		}

		public static IServiceCollection AddCommandClient(this IServiceCollection services) => AddCommandClient<CommandClient>(services);

		//public static bool TryAddCommandHandler(this IServiceCollection services, Type commandHandlerType, Func<object, IServiceProvider, string> getQueueName) {
		//	if (commandHandlerType.TryGetClosedGenericType(typeof(ICommandHandler<,>), out Type? genericType)) {
		//		var genericArguments = genericType.GetGenericArguments();
		//		services.AddCommand(genericArguments[0], genericArguments[1], getQueueName);
		//		services.TryAddScoped(genericType, commandHandlerType);
		//	} else if (commandHandlerType.TryGetClosedGenericType(typeof(ICommandHandler<>), out genericType)) {
		//		var genericArguments = genericType.GetGenericArguments();
		//		services.AddCommand(genericArguments[0], null, getQueueName);
		//		services.TryAddScoped(genericType, commandHandlerType);
		//	} else {
		//		return false;
		//	}
		//	return true;
		//}
		public static IServiceCollection AddAssemblyCommandHandlers(this IServiceCollection services, Assembly assembly, Func<object, IServiceProvider, string> getQueueName) {
			var types = assembly.GetConcreteClasses<ICommandHandler>();
			foreach (var type in types) {
				// TryAddCommandHandler(services, type, getQueueName);
			}
			return services;
		}

		public static IServiceCollection AddCommandBus(this IServiceCollection services) {
			services.TryAddSingleton<ICommandBusService, CommandBusService>();
			services.AddSingleton<IRouterServerService>(provider => provider.GetRequiredService<ICommandBusService>());
			services.AddSingleton<IRouterServerService, CommandBusReplayService>();
			services.TryAddSingleton<ICommandQueueFactory, CommandQueueFactory>();
			services.TryAddTransient<CommandQueue>();
			services.TryAddScoped<InternalCommandClient>();
			services.TryAddScoped<CommandContext>();
			services.AddRouterServer();
			return services;
		}

		public static Task SubmitCollection(this ICommandClient client, IEnumerable<object> commands, bool fireAndForget = true, int timeout = 2000) {
			var tasks = new List<Task>();
			foreach (var cmd in commands) {
				tasks.Add(client.Submit(cmd, fireAndForget, 0));
			}
			return tasks.WithTimeOut(TimeSpan.FromMilliseconds(timeout));
		}
	}
}