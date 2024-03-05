using Albatross.Caching.BuiltIn;
using Albatross.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly.Registry;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Caching {
	// class name should not be renamed to Extensions due to backward compatibilies issue
	public static class Extension {
		public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration) {
			var registry = new PolicyRegistry();
			services.TryAdd(ServiceDescriptor.Singleton<IPolicyRegistry<string>>(registry));
			services.TryAdd(ServiceDescriptor.Singleton<IReadOnlyPolicyRegistry<string>>(registry));
			return services;
		}

		public static IServiceCollection AddBuiltInCache(this IServiceCollection services) {
			services.AddSingleton(typeof(OneSecondCache<,>));
			services.AddSingleton(typeof(FiveSecondsCache<,>));
			services.AddSingleton(typeof(TenSecondsCache<,>));
			services.AddSingleton(typeof(FifteenSecondsCache<,>));

			services.AddSingleton(typeof(OneMinuteCache<,>));
			services.AddSingleton(typeof(FiveMinutesCache<,>));
			services.AddSingleton(typeof(TenMinutesCache<,>));
			services.AddSingleton(typeof(FifteenMinutesCache<,>));

			services.AddSingleton(typeof(OneDayCache<,>));
			services.AddSingleton(typeof(OneMonthCache<,>));

			services.AddSingleton(typeof(OneSecondSlidingTtlCache<,>));
			return services;
		}

		public static void Remove(this ICacheKeyManagement keyMgmt, params ICacheKey[] keys) {
			var set = new HashSet<string>();
			set.AddRange(keys.Select(x => x.Key));
			keyMgmt.Remove(set.ToArray());
		}
		public static void RemoveSelfAndChildren(this ICacheKeyManagement keyMgmt, params ICacheKey[] keys) {
			var set = new HashSet<string>();
			foreach (var key in keys) {
				set.AddRange(keyMgmt.FindKeys(key.WildCardKey));
			}
			keyMgmt.Remove(set.ToArray());
		}
		public static void RemoveSelfAndChildren(this ICacheKeyManagement keyMgmt, params ICachedObject[] cachedObjects) {
			var keys = cachedObjects.SelectMany(x => x.CacheKeys).ToArray();
			keyMgmt.RemoveSelfAndChildren(keys);
		}
		public static void Reset(this ICacheKeyManagement keyMgmt, params ICacheKey[] keys) {
			var set = new HashSet<string>();
			foreach (var key in keys) {
				set.AddRange(keyMgmt.FindKeys(key.ResetKey));
			}
			keyMgmt.Remove(set.ToArray());
		}
	}
}
