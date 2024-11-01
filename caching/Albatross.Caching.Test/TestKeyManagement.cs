using Albatross.Caching.BuiltIn;
using Albatross.Caching.Test.CacheKeys;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestKeyManagement {
		[Theory]
		[InlineData(My.MemCacheHostType)]
		[InlineData(My.RedisCacheHostType)]
		public async Task TestStringKeyCreation(string hostType) {
			using var host = My.GetTestHost(hostType);
			using var scope = host.Services.CreateScope();
			var cache1 = scope.ServiceProvider.GetRequiredService<OneDayCache<MyData, CacheKey>>();
			var keyMgmt = scope.ServiceProvider.GetRequiredService<ICacheKeyManagement>();

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
		[InlineData(My.MemCacheHostType)]
		[InlineData(My.RedisCacheHostType)]
		public async Task TestKeyRemoval(string hostType) {
			using var host = My.GetTestHost(hostType);
			using var scope = host.Services.CreateScope();
			var cache = scope.ServiceProvider.GetRequiredService<OneDayCache<MyData, CacheKey>>();
			var keyMgmt = scope.ServiceProvider.GetRequiredService<ICacheKeyManagement>();

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
		[InlineData(My.MemCacheHostType)]
		[InlineData(My.RedisCacheHostType)]
		public async Task TestKeyReset(string hostType) {
			using var host = My.GetTestHost(hostType);
			using var scope = host.Services.CreateScope();
			var stringKeyCache = scope.ServiceProvider.GetRequiredService<OneDayCache<MyData, CustomKey1>>();
			var stringKey2Cache = scope.ServiceProvider.GetRequiredService<OneDayCache<MyData, CustomKey2>>();
			var keyMgmt = scope.ServiceProvider.GetRequiredService<ICacheKeyManagement>();

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