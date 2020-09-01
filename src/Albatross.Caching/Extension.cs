using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Transactions;

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
			return services;
		}

		public static void UseCache(this IServiceProvider serviceProvider) {
			IPolicyRegistry<string> registry = serviceProvider.GetRequiredService<IPolicyRegistry<string>>();
			IAsyncCacheProvider cacheProvider = serviceProvider.GetRequiredService<IAsyncCacheProvider>();
			var items = serviceProvider.GetRequiredService<IEnumerable<ICacheManagement>>();
			foreach (var item in items) {
				item.Register(registry, cacheProvider);
			}
		}

		public static IServiceCollection AddCacheMgmt<T>(this IServiceCollection services) where T: class, ICacheManagement {
			services.AddSingleton<ICacheManagement, T>();
			return services;
		}
		public static IAsyncPolicy<T> GetAsyncPolicy<T>(this IReadOnlyPolicyRegistry<string> registry, string key) => registry.Get<IAsyncPolicy<T>>(key);
	}
}
