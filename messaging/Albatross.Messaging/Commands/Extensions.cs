using Albatross.Messaging.Services;
using Albatross.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	public static class Extensions {
		public const string DefaultQueueName = "default_queue";
		public static string GetDefaultQueueName(ulong messageId, object _, IServiceProvider provider) => DefaultQueueName;

		public static IServiceCollection AddCommand<T>(this IServiceCollection services, string[] names, Func<ulong, T, IServiceProvider, string> getQueueName) where T : notnull {
			services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IRegisterCommand), new RegisterCommand<T>(names, getQueueName)));
			return services;
		}
		public static IServiceCollection AddCommand<T, K>(this IServiceCollection services, string[] names, Func<ulong, T, IServiceProvider, string> getQueueName) where T : notnull where K : notnull {
			services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IRegisterCommand), new RegisterCommand<T, K>(names, getQueueName)));
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