using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public static class Extension {
		public static void CreateCachingPolicy<T>(this IServiceProvider provider, string cacheKey, ITtlStrategy strategy) {
			IPolicyRegistry<string> registry = provider.GetRequiredService<IPolicyRegistry<string>>();
			IAsyncPolicy<T> policy = Policy.CacheAsync<T>(provider.GetRequiredService<IAsyncCacheProvider>().AsyncFor<T>(), strategy);
			registry.Add(cacheKey, policy);
		}

		public static IServiceCollection AddCaching(this IServiceCollection services) {
			var registry = new PolicyRegistry();
			services.AddMemoryCache();
			services.AddSingleton<IPolicyRegistry<string>>(registry);
			services.AddSingleton<IReadOnlyPolicyRegistry<string>>(registry);
			services.AddSingleton<IAsyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>();
			services.AddSingleton<ICacheManagementFactory, CacheManagementFactory>();
			return services;
		}

		public static void UseCache(this IServiceProvider serviceProvider, ILogger logger) {
			IPolicyRegistry<string> registry = serviceProvider.GetRequiredService<IPolicyRegistry<string>>();
			IAsyncCacheProvider cacheProvider = serviceProvider.GetRequiredService<IAsyncCacheProvider>();
			var items = serviceProvider.GetRequiredService<IEnumerable<ICacheManagement>>();
			foreach (var item in items) {
				logger.LogInformation("Register Cache Management {cacheName}", item.Name);
				item.Register();
			}
		}

		public static IServiceCollection AddCacheMgmt<T>(this IServiceCollection services) where T: class, ICacheManagement {
			services.AddSingleton<ICacheManagement, T>();
			return services;
		}

		/// <summary>
		/// Register all ICacheManagement implementation of an assembly using the singleton scope.  The search of the concrete class is performed using the generic type ICacheManagement<>
		/// </summary>
		/// <param name="services"></param>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public static IServiceCollection AddCacheMgmt(this IServiceCollection services, Assembly assembly){
			if (assembly != null) {
				Type genericDefinition = typeof(ICacheManagement<>);
				foreach (Type type in assembly.GetConcreteClasses()) {
					if (type.TryGetClosedGenericType(genericDefinition, out Type _)) {
						services.AddSingleton(typeof(ICacheManagement), type);
					}
				}
			}
			return services;
		}

		public static IAsyncPolicy<T> GetAsyncPolicy<T>(this IReadOnlyPolicyRegistry<string> registry, string key) => registry.Get<IAsyncPolicy<T>>(key);
	}
}
