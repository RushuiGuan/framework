using Albatross.Caching.BuiltIn;
using Albatross.Caching.Test.CacheKeys;
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
			var keyValues = new string[] { "", "1", "2", "3" };
			var keys = new List<ICacheKey>();

			foreach (var keyValue in keyValues) {
				var key = new CacheKey(keyValue, false);
				keys.Add(key);
				await cache1.PutAsync(key, data);
			}
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var keyObject in keys) {
				Assert.Contains(keyObject.Key, allKeys);
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
				await cache.PutAsync(new CacheKey(keyValue, true), data);
			}
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var keyValue in keyValues) {
				Assert.Contains(new CacheKey(keyValue, true).Key, allKeys);
			}
			foreach (var keyValue in keyValues) {
				keyMgmt.Remove(new CacheKey(keyValue, true));
			}
			allKeys = keyMgmt.FindKeys("*");
			foreach (var keyValue in keyValues) {
				Assert.DoesNotContain(new CacheKey(keyValue, true).Key, allKeys);
			}
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestKeyReset(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var stringKeyCache = scope.Get<OneDayCache<MyData, CustomKey1>>();
			var stringKey2Cache = scope.Get<OneDayCache<MyData, CustomKey2>>();
			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var data = new MyData("a");
			var keyValues = new string[] { "", "1", "2", "3" };
			var keys = new List<ICacheKey>();

			foreach (var keyValue in keyValues) {
				ICacheKey key = new CustomKey1(keyValue);
				keys.Add(key);
				await stringKeyCache.PutAsync((CustomKey1)key, data);

				key = new CustomKey2(keyValue);
				keys.Add(key);
				await stringKey2Cache.PutAsync((CustomKey2)key, data);
			}

			var allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.Contains(key.Key, allKeys);
			}
			keyMgmt.Reset(new CustomKey1(null));
			allKeys = keyMgmt.FindKeys("*");
			foreach (var keyValue in keyValues) {
				Assert.DoesNotContain(new CustomKey1(keyValue).Key, allKeys);
			}
			foreach (var keyValue in keyValues) {
				Assert.Contains(new CustomKey2(keyValue).Key, allKeys);
			}
		}
	}
}