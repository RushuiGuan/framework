using Albatross.Caching.BuiltIn;
using Albatross.Caching.Test.CacheMgmt;
using Albatross.Hosting.Test;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestKeyManagement {
		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestStringKeyCreation(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var cache1 = scope.Get<OneDayCache<MyData, CacheKey>>();
			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var data = new MyData("a");
			var keys = new string[] { "", "1", "2", "3" };

			foreach (var key in keys) {
				await cache1.PutAsync(new CacheKey(key), data);
			}
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.Contains(new CacheKey(key).Key, allKeys);
			}
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestKeyRemoval(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var cache = scope.Get<OneDayCache<MyData, CacheKey>>();
			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var data = new MyData("a");
			var keyValues = new string[] { "", "1", "2", "3" };

			foreach (var keyValue in keyValues) {
				await cache.PutAsync(new CacheKey(keyValue), data);
			}
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var keyValue in keyValues) {
				Assert.Contains(new CacheKey(keyValue).Key, allKeys);
			}
			foreach (var keyValue in keyValues) {
				keyMgmt.Remove(new CacheKey(keyValue));
			}
			allKeys = keyMgmt.FindKeys("*");
			foreach (var keyValue in keyValues) {
				Assert.DoesNotContain(new CacheKey(keyValue).Key, allKeys);
			}
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestKeyReset(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var stringKeyCache = scope.Get<OneDayCache<MyData, StringKey>>();
			var stringKey2Cache = scope.Get<OneDayCache<MyData, StringKey2>>();
			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var data = new MyData("a");
			var keyValues = new string[] { "", "1", "2", "3" };
			var keys = new List<ICacheKey>();

			foreach (var keyValue in keyValues) {
				ICacheKey key = new StringKey(keyValue);
				keys.Add(key);
				await stringKeyCache.PutAsync((StringKey)key, data);

				key = new StringKey2(keyValue);
				keys.Add(key);
				await stringKey2Cache.PutAsync((StringKey2)key, data);
			}

			var allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.Contains(key.Key, allKeys);
			}
			keyMgmt.Reset(new StringKey(null));
			allKeys = keyMgmt.FindKeys("*");
			foreach (var keyValue in keyValues) {
				Assert.DoesNotContain(new StringKey(keyValue).Key, allKeys);
			}
			foreach (var keyValue in keyValues) {
				Assert.Contains(new StringKey2(keyValue).Key, allKeys);
			}
		}
	}
}