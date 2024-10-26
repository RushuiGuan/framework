using Albatross.Caching.BuiltIn;
using Microsoft.Extensions.DependencyInjection;
using Sample.Caching.WebApi.CacheKeys;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestCacheWithNoKeyValue {
		[Fact]
		public void TestKeyGeneration() {
			var k1Key = new Level1Key(string.Empty);
			var k2Key = new Level2Key(string.Empty, string.Empty);
			var k3Key = new Level3Key(string.Empty, string.Empty, string.Empty);
			Assert.Equal("k1:", k1Key.Key);
			Assert.Equal("k1:*", k1Key.ResetKey);
			Assert.Equal("k1:*", k1Key.WildCardKey);

			Assert.Equal("k1:k2:", k2Key.Key);
			Assert.Equal("k1:k2:*", k2Key.ResetKey);
			Assert.Equal("k1:k2:*", k2Key.WildCardKey);

			Assert.Equal("k1:k2:k3:", k3Key.Key);
			Assert.Equal("k1:k2:k3:*", k3Key.WildCardKey);
			Assert.Equal("k1:k2:k3:*", k3Key.ResetKey);
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestBasicOperation(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var tier1 = scope.ServiceProvider.GetRequiredService<OneDayCache<int, CacheKey>>();

			await tier1.PutAsync(new CacheKey(null, true), 1);
			var result = await tier1.TryGetAsync(new CacheKey(null, true));
			Assert.True(result.Item1);
			Assert.Equal(1, result.Item2);
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestLeveledKeyRemoveSelfOnly(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var keyMgmt = scope.ServiceProvider.GetRequiredService<ICacheKeyManagement>();
			var tier1 = scope.ServiceProvider.GetRequiredService<OneDayCache<int, Level1Key>>();
			var tier2 = scope.ServiceProvider.GetRequiredService<OneDayCache<int, Level2Key>>();
			var tier3 = scope.ServiceProvider.GetRequiredService<OneDayCache<int, Level3Key>>();

			var level1 = new Level1Key(string.Empty);
			var level2 = new Level2Key(string.Empty, string.Empty);
			var level3 = new Level3Key(string.Empty, string.Empty, string.Empty);

			keyMgmt.Reset(level1);

			await tier1.PutAsync(level1, 1);
			await tier2.PutAsync(level2, 2);
			await tier3.PutAsync(level3, 3);

			// base verification that all keys are created
			var allKeys = keyMgmt.FindKeys(level1.ResetKey);
			Assert.Contains(level1.Key, allKeys);
			Assert.Contains(level2.Key, allKeys);
			Assert.Contains(level3.Key, allKeys);

			keyMgmt.Remove(level3.Key);
			allKeys = keyMgmt.FindKeys(level1.ResetKey);
			Assert.DoesNotContain(level3.Key, allKeys);
			Assert.Contains(level2.Key, allKeys);
			Assert.Contains(level1.Key, allKeys);

			keyMgmt.Remove(level2.Key);
			allKeys = keyMgmt.FindKeys(level1.ResetKey);
			Assert.DoesNotContain(level2.Key, allKeys);
			Assert.Contains(level1.Key, allKeys);

			keyMgmt.Remove(level1.Key);
			allKeys = keyMgmt.FindKeys(level1.ResetKey);
			Assert.DoesNotContain(level1.Key, allKeys);
		}

		[Theory]
		[InlineData(MemCacheHost.HostType, 1)]
		[InlineData(MemCacheHost.HostType, 2)]
		[InlineData(MemCacheHost.HostType, 3)]

		[InlineData(RedisCacheHost.HostType, 1)]
		[InlineData(RedisCacheHost.HostType, 2)]
		[InlineData(RedisCacheHost.HostType, 3)]
		public async Task TestLeveledKeyRemoveSelfAndChildren(string hostType, int tier) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var keyMgmt = scope.ServiceProvider.GetRequiredService<ICacheKeyManagement>();
			var tier1 = scope.ServiceProvider.GetRequiredService<OneDayCache<int, Level1Key>>();
			var tier2 = scope.ServiceProvider.GetRequiredService<OneDayCache<int, Level2Key>>();
			var tier3 = scope.ServiceProvider.GetRequiredService<OneDayCache<int, Level3Key>>();

			var level1 = new Level1Key(string.Empty);
			var level2 = new Level2Key(string.Empty, string.Empty);
			var level3 = new Level3Key(string.Empty, string.Empty, string.Empty);

			keyMgmt.Reset(level1);

			await tier1.PutAsync(level1, 1);
			await tier2.PutAsync(level2, 1);
			await tier3.PutAsync(level3, 1);

			// base verification that all keys are created
			var allKeys = keyMgmt.FindKeys(level1.ResetKey);
			Assert.Contains(level1.Key, allKeys);
			Assert.Contains(level2.Key, allKeys);
			Assert.Contains(level3.Key, allKeys);

			// remove the target key from the selected tier
			switch (tier) {
				case 1:
					keyMgmt.RemoveSelfAndChildren(level1);
					break;
				case 2:
					keyMgmt.RemoveSelfAndChildren(level2);
					break;
				case 3:
					keyMgmt.RemoveSelfAndChildren(level3);
					break;
			}
			// get all keys again
			allKeys = keyMgmt.FindKeys(level1.ResetKey);
			if (tier == 1) {
				Assert.Empty(allKeys);
			} else if (tier == 2) {
				Assert.Contains(level1.Key, allKeys);
				Assert.DoesNotContain(level2.Key, allKeys);
				Assert.DoesNotContain(level3.Key, allKeys);
			} else if (tier == 3) {
				Assert.Contains(level1.Key, allKeys);
				Assert.Contains(level2.Key, allKeys);
				Assert.DoesNotContain(level3.Key, allKeys);
			}
		}
	}
}