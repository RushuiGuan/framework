﻿using Albatross.Threading;
using Albatross.Reflection;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

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
			foreach (var type in assembly.GetConcreteClasses()) {
				var attrib = type.GetCustomAttribute<CommandAttribute>();
				if (attrib != null) {
					services.AddSingleton<IRegisterCommand>(new RegisterCommand(type, attrib.ResponseType, getQueueName ?? ((_, provider) => DefaultQueueName)));
				}
			}
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

		public static Task SubmitCollection(this ICommandClient client, IEnumerable<object> commands, bool fireAndForget = true, int timeout = 2000) {
			var tasks = new List<Task>();
			foreach (var cmd in commands) {
				tasks.Add(client.Submit(cmd, fireAndForget, 0));
			}
			return tasks.WithTimeOut(TimeSpan.FromMilliseconds(timeout));
		}
	}
}