using Albatross.Caching.BuiltIn;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly.Registry;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Caching {
	public static class Extensions {
		public static IServiceCollection AddCaching(this IServiceCollection services) {
			var registry = new PolicyRegistry();
			services.TryAdd(ServiceDescriptor.Singleton<IPolicyRegistry<string>>(registry));
			services.TryAdd(ServiceDescriptor.Singleton<IReadOnlyPolicyRegistry<string>>(registry));
			return services;
		}

		public static IServiceCollection AddBuiltInCache(this IServiceCollection services) {
			services.TryAddSingleton(typeof(OneSecondCache<,>));
			services.TryAddSingleton(typeof(FiveSecondsCache<,>));
			services.TryAddSingleton(typeof(TenSecondsCache<,>));
			services.TryAddSingleton(typeof(FifteenSecondsCache<,>));

			services.TryAddSingleton(typeof(OneMinuteCache<,>));
			services.TryAddSingleton(typeof(FiveMinutesCache<,>));
			services.TryAddSingleton(typeof(TenMinutesCache<,>));
			services.TryAddSingleton(typeof(FifteenMinutesCache<,>));

			services.TryAddSingleton(typeof(OneHourCache<,>));

			services.TryAddSingleton(typeof(OneDayCache<,>));
			services.TryAddSingleton(typeof(OneMonthCache<,>));
			services.TryAddSingleton(typeof(OneYearCache<,>));

			services.TryAddSingleton(typeof(OneSecondSlidingTtlCache<,>));
			return services;
		}

		public static IEnumerable<string> Remove(this ICacheKeyManagement keyMgmt, params ICacheKey[] keys) {
			var set = new HashSet<string>(keys.Select(x=>x.Key));
			keyMgmt.Remove(set.ToArray());
			return set;
		}
		public static IEnumerable<string> RemoveSelfAndChildren(this ICacheKeyManagement keyMgmt, params ICacheKey[] keys) {
			var wildCardKeys = new HashSet<string>();
			var set = new HashSet<string>();
			foreach (var key in keys) {
				if (key.HasChildren) {
					if (!wildCardKeys.Contains(key.WildCardKey)) {
						wildCardKeys.Add(key.WildCardKey);
						foreach(var item in keyMgmt.FindKeys(key.WildCardKey)) {
							set.Add(item);
						}
					}
				} else {
					set.Add(key.Key);
				}
			}
			keyMgmt.Remove(set.ToArray());
			return set;
		}
		public static IEnumerable<string> Reset(this ICacheKeyManagement keyMgmt, params ICacheKey[] keys) {
			var wildCardKeys = new HashSet<string>();
			var set = new HashSet<string>();
			foreach (var key in keys) {
				if (!wildCardKeys.Contains(key.ResetKey)) {
					wildCardKeys.Add(key.ResetKey);
					foreach(var item in keyMgmt.FindKeys(key.ResetKey)) {
						set.Add(item);
					}
				}
			}
			keyMgmt.Remove(set.ToArray());
			return set;
		}
	}
}
