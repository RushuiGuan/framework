using Albatross.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Albatross.Caching {
	// class name should not be renamed to Extensions due to backward compatibilies issue
	public static class Extension {
		public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration) {
			var registry = new PolicyRegistry();
			services.TryAdd(ServiceDescriptor.Singleton<IPolicyRegistry<string>>(registry));
			services.TryAdd(ServiceDescriptor.Singleton<IReadOnlyPolicyRegistry<string>>(registry));
			services.TryAdd(ServiceDescriptor.Singleton<ICacheManagementFactory, CacheManagementFactory>());
			return services;
		}

		public static void UseCache(this IServiceProvider serviceProvider) {
			var items = serviceProvider.GetRequiredService<IEnumerable<ICacheManagement>>();
			var logger = serviceProvider.GetRequiredService<ILogger>();
			foreach (var item in items) {
				logger.LogInformation("Register Cache Management {cacheName}", item.Name);
				item.Register();
			}
		}

		public static IServiceCollection AddCacheMgmt<T>(this IServiceCollection services) where T : class, ICacheManagement {
			services.TryAddSingleton<T>();
			services.TryAddEnumerable(ServiceDescriptor.Singleton<ICacheManagement>(provider => provider.GetRequiredService<T>()));
			return services;
		}

		/// <summary>
		/// Register all ICacheManagement implementation of an assembly using the singleton scope.  The search of the concrete class is performed using the generic type ICacheManagement<>
		/// </summary>
		/// <param name="services"></param>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public static IServiceCollection AddCacheMgmt(this IServiceCollection services, Assembly assembly) {
			if (assembly != null) {
				Type genericDefinition = typeof(ICacheManagement<,>);
				foreach (Type type in assembly.GetConcreteClasses()) {
					if (type.TryGetClosedGenericType(genericDefinition, out Type? _)) {
						services.TryAddSingleton(type);
						services.AddSingleton<ICacheManagement>(provider => (ICacheManagement)provider.GetRequiredService(type));
					}
				}
			}
			return services;
		}
	}
}
