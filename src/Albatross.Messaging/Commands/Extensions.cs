﻿using Albatross.Reflection;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Albatross.Messaging.DataLogging;

namespace Albatross.Messaging.Commands {
	public static class Extensions {
		static string GetDefaultQueueName<CommandType>(CommandType _, IServiceProvider provider) where CommandType : notnull => "default_queue";

		public static IServiceCollection AddCommand<T>(this IServiceCollection services, Func<T, IServiceProvider, string>? getQueueName = null) where T : notnull {
			services.AddSingleton<IRegisterCommand>(provider => new RegisterCommand<T>(getQueueName ?? GetDefaultQueueName));
			return services;
		}

		public static IServiceCollection AddCommand<T, K>(this IServiceCollection services, Func<T, IServiceProvider, string>? getQueueName = null) where T : notnull where K: notnull{
			services.AddSingleton<IRegisterCommand>(provider => new RegisterCommand<T, K>(getQueueName ?? GetDefaultQueueName));
			return services;
		}

		public static IServiceCollection AddCommandClient(this IServiceCollection services) {
			services.TryAddSingleton<CommandClientService>();
			services.AddSingleton<IDealerClientService>(args=>args.GetRequiredService<CommandClientService>());
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