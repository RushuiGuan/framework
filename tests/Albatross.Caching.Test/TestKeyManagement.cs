using Albatross.Caching.Test.CacheMgmt;
using Albatross.Hosting.Test;
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
			var cache1 = scope.Get<StringKeyCacheMgmt>();
			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var data = new MyData("a");
			var keys = new string[] { "", "1", "2", "3" };

			foreach (var key in keys) {
				await cache1.PutAsync(key, data);
			}
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.Contains(cache1.CreateKey(key), allKeys);
			}
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestKeyRemoval(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var cache = scope.Get<StringKeyCacheMgmt>();
			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var data = new MyData("a");
			var keys = new string[] { "", "1", "2", "3" };

			foreach (var key in keys) {
				await cache.PutAsync(key, data);
			}
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.Contains(cache.CreateKey(key), allKeys);
			}
			foreach (var key in keys) {
				cache.Remove(key);
			}
			allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.DoesNotContain(cache.CreateKey(key), allKeys);
			}
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestKeyReset(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var compositeKeycache = scope.Get<StringKey2CacheMgmt>();
			var stringKeyCache = scope.Get<StringKeyCacheMgmt>();
			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var data = new MyData("a");
			var keys = new string[] { "", "1", "2", "3" };

			foreach (var key in keys) {
				await compositeKeycache.PutAsync(key, data);
				await stringKeyCache.PutAsync(key, data);
			}

			var allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.Contains(compositeKeycache.CreateKey(key), allKeys);
			}
			compositeKeycache.RemoveSelfAndChildren(string.Empty);

			allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.DoesNotContain(compositeKeycache.CreateKey(key), allKeys);
			}
			foreach (var key in keys) {
				Assert.Contains(stringKeyCache.CreateKey(key), allKeys);
			}
		}
	}
}