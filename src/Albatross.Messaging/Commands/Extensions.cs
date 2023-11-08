using Albatross.Reflection;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

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
			services.AddSingleton<IDealerClientService>(args=>args.GetRequiredService<CommandClientService>());
			services.TryAddSingleton<ICommandClient, CommandClient>();
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
			services.TryAddSingleton<ICommandQueueFactory, CommandQueueFactory>();
			services.TryAddTransient<CommandQueue>();
			// this should only be used if the TaskCommandQueue is used
			services.TryAddSingleton<InternalCommandClient>();
			services.TryAddScoped<CommandContext>();
			services.AddRouterServer();
			return services;
		}

		public static IEnumerable<ulong> SubmitCollection(this ICommandClient client, IEnumerable<object> commands) {
			foreach(var cmd in commands) {
				yield return client.Submit(cmd);
			}
		}

		public static void CreateCommandContext(this IServiceProvider provider, CommandQueueItem item) {
			var context = provider.GetRequiredService<CommandContext>();
			context.OriginalRoute = item.Route;
			context.OriginalId = item.Id;
			context.Queue = item.Queue.Name;
			context.IsInternal = item.Route == InternalCommand.Route;
		}
	}
}