using Albatross.Config;
using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

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

			services.TryAdd(ServiceDescriptor.Singleton<IPolicyRegistry<string>>(registry));
			services.TryAdd(ServiceDescriptor.Singleton<IReadOnlyPolicyRegistry<string>>(registry));
			services.TryAdd(ServiceDescriptor.Singleton<IAsyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>());
			services.TryAdd(ServiceDescriptor.Singleton<ICacheManagementFactory, CacheManagementFactory>());
			services.TryAdd(ServiceDescriptor.Singleton<IMemoryCacheExtended, MemoryCacheExtended>());
			services.AddConfig<CachingConfig>(true);
			services.AddHttpClient(CachingClient.CachingProxyName).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseDefaultCredentials = true, });
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


		public static ICacheManagement Get(this ICacheManagementFactory factory, string name) {
			if(factory.TryGetValue(name, out ICacheManagement result)) {
				return result;
			} else {
				throw new ArgumentException($"CacheManagement {name} is not registered");
			}
		}

		public static ICacheManagement<CacheFormat> Get<CacheFormat>(this ICacheManagementFactory factory, string name) {
			ICacheManagement cache = factory.Get(name);
			return (ICacheManagement<CacheFormat>)cache;
		}

		public static void Evict(this ICacheManagementFactory factory, string cacheName, params string[] keys) {
			var mgmt = factory.Get(cacheName);
			mgmt.Evict(keys.Select(args => new Context(args)).ToArray());
		}
	}
}
