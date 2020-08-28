using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Hosting {
	public static class Extension {
		/// <summary>
		/// Create a generic async caching policy using the full name of the generic type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="registry"></param>
		/// <param name="serviceProvider"></param>
		/// <param name="ttl">fixed ttl in timespan</param>
		/// <param name="strategy">ttl strategy, if null, the time span ttl parameter will be used</param>
		public static PolicyRegistry CreateCachingPolicyByType<T>(this PolicyRegistry registry, IServiceProvider serviceProvider, TimeSpan ttl, ITtlStrategy strategy) {
			IAsyncPolicy<T> policy;
			if (strategy == null) {
				policy = Policy.CacheAsync<T>(serviceProvider.GetRequiredService<IAsyncCacheProvider>().AsyncFor<T>(), ttl);
			} else {
				policy = Policy.CacheAsync<T>(serviceProvider.GetRequiredService<IAsyncCacheProvider>().AsyncFor<T>(), strategy);
			}
			string key = typeof(T).FullName;
			registry.Add(key, policy);
			return registry;
		}

		public static IAsyncPolicy<T> GetPolicyByType<T>(this IReadOnlyPolicyRegistry<string> registry) {
			string key = typeof(T).FullName;
			return registry.Get<IAsyncPolicy<T>>(key);
		}
	}
}
