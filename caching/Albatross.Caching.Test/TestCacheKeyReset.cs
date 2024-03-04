using Albatross.Hosting.Test;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using Albatross.Caching.Test.CacheMgmt;
using Albatross.Caching.BuiltIn;

namespace Albatross.Caching.Test {
	public class TestCacheKeyReset {
		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestResetOperation(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var keyMgmt = scope.Get<ICacheKeyManagement>();
			var cache = scope.ServiceProvider.GetRequiredService<OneDayCache<MyData, CacheKey>>();
			keyMgmt.Reset(new CacheKey(null));

			var keys = new List<CacheKey>();
			for (int x = 0; x < 10; x++) {
				var cacheKey = new CacheKey(x.ToString());
				keys.Add(cacheKey);
				await cache.PutAsync(cacheKey, new MyData());
			}

			// base verification that all keys are created
			var allKeys = keyMgmt.FindKeys(new CacheKey(null).WildCardKey);
			foreach (var item in keys) {
				Assert.Contains(item.Key, allKeys);
			}
			// reset the key
			keyMgmt.Reset(new CacheKey(null));
			allKeys = keyMgmt.FindKeys(new CacheKey(null).WildCardKey);
			foreach (var item in keys) {
				Assert.DoesNotContain(item.Key, allKeys);
			}
		}
	}
}
