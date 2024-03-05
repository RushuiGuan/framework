using Albatross.Hosting.Test;
using Sample.Caching.WebApi;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using Albatross.Caching.BuiltIn;
using Sample.Caching.WebApi.CacheKeys;
using System.Linq;

namespace Albatross.Caching.Test {
	public class TestMultiTierCache {
		[Fact]
		public void TestKeyGeneration() {
			using var host = "redis".GetTestHost();
			using var scope = host.Create();
			
			Assert.Equal("t1:1:", new Tier1Key(1).Key);
			Assert.Equal("t1:1:*", new Tier1Key(1).WildCardKey);
			Assert.Equal("t1:*", new Tier1Key(1).ResetKey);

			Assert.Equal("t1:1:t2:2:", new Tier2Key(1, 2).Key);
			Assert.Equal("t1:1:t2:2:*", new Tier2Key(1, 2).WildCardKey);
			Assert.Equal("t1:1:t2:*", new Tier2Key(1, 2).ResetKey);

			Assert.Equal("t1:1:t2:2:t3:3:", new Tier3Key(1, 2, 3).Key);
			Assert.Equal("t1:1:t2:2:t3:3:*", new Tier3Key(1, 2, 3).WildCardKey);
			Assert.Equal("t1:1:t2:2:t3:*", new Tier3Key(1, 2, 3).ResetKey);
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestBasicOperation(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var tier1Cache = scope.ServiceProvider.GetRequiredService<OneDayCache<string, Tier1Key>>();

			var keys = new Tier1Key[] {
				new Tier1Key(1),
				new Tier1Key(2),
				new Tier1Key(3),
				new Tier1Key(4),
				new Tier1Key(5),
			};

			foreach (var key in keys) {
				await tier1Cache.PutAsync(key, key.ToString());
			}

			foreach (var key in keys) {
				var result = await tier1Cache.TryGetAsync(key, CancellationToken.None);
				Assert.True(result.Item1);
				Assert.Equal(key.ToString(), result.Item2);
			}
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestTieredKeyRemoveSelfOnly(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var keyMgmt = scope.Get<ICacheKeyManagement>();
			var tier1 = scope.ServiceProvider.GetRequiredService<OneDayCache<string, Tier1Key>>();
			var tier2 = scope.ServiceProvider.GetRequiredService<OneDayCache<string, Tier2Key>>();
			var tier3 = scope.ServiceProvider.GetRequiredService<OneDayCache<string, Tier3Key>>();
			keyMgmt.Reset(new Tier1Key(0));
			
			var keys = new List<ICacheKey>();
			for (int x = 0; x < 3; x++) {
				for (int y = 0; y < 3; y++) {
					for (int z = 0; z < 3; z++) {
						var tier3Key = new Tier3Key(x, y, z);
						var tier2Key = new Tier2Key(x, y);
						var tier1Key = new Tier1Key(x);
						keys.Add(tier3Key);
						keys.Add(tier2Key);
						keys.Add(tier1Key);

						await tier1.PutAsync(tier1Key, tier1Key.ToString());
						await tier2.PutAsync(tier2Key, tier2Key.ToString());
						await tier3.PutAsync(tier3Key, tier3Key.ToString());
					}
				}
			}

			// base verification that all keys are created
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var item in keys) {
				Assert.Contains(item.Key, allKeys);
			}
			// remove one key at a time and make sure only that key is removed
			foreach (var item in keys) {
				keyMgmt.Remove(item.Key);
				allKeys = keyMgmt.FindKeys("*");
				Assert.DoesNotContain(item.Key, allKeys);
			}
		}

		[Theory]
		[InlineData(MemCacheHost.HostType, 1, 1, 1, 1)]
		[InlineData(RedisCacheHost.HostType, 1, 1, 1, 1)]
		[InlineData(RedisCacheHost.HostType, 2, 1, 1, 1)]
		[InlineData(RedisCacheHost.HostType, 3, 1, 1, 1)]
		[InlineData(RedisCacheHost.HostType, 1, 1, 2, 3)]
		[InlineData(RedisCacheHost.HostType, 2, 1, 2, 3)]
		[InlineData(RedisCacheHost.HostType, 3, 1, 2, 3)]
		public async Task TestTieredKeyRemoveSelfAndChildren(string hostType, int tier, int keyValue1, int keyValue2, int keyValue3) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var tier1Key = new Tier1Key(keyValue1);
			var tier2Key = new Tier2Key(keyValue1, keyValue2);
			var tier3Key = new Tier3Key(keyValue1, keyValue2, keyValue3);

			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var tier1 = scope.ServiceProvider.GetRequiredService<OneDayCache<string, Tier1Key>>();
			var tier2 = scope.ServiceProvider.GetRequiredService<OneDayCache<string, Tier2Key>>();
			var tier3 = scope.ServiceProvider.GetRequiredService<OneDayCache<string, Tier3Key>>();

			keyMgmt.Reset(tier1Key);

			var keys = new List<ICacheKey>();
			for (int x = 0; x < 3; x++) {
				for (int y = 0; y < 3; y++) {
					for (int z = 0; z < 3; z++) {
						var t3Key = new Tier3Key(x, y, z);
						keys.Add(t3Key);
						await tier3.PutAsync(t3Key, t3Key.ToString());

						var t2Key = new Tier2Key(x, y);
						await tier2.PutAsync(t2Key, t2Key.ToString());
						keys.Add(t2Key);

						var t1Key = new Tier1Key(x);
						await tier1.PutAsync(t1Key, t1Key.ToString());
						keys.Add(t1Key);
					}
				}
			}
			// base verification that all keys are created
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var item in keys) {
				Assert.Contains(item.Key, allKeys);
			}
			// remove the target key from the selected tier
			switch (tier) {
				case 1:
					keyMgmt.RemoveSelfAndChildren(tier1Key);
					break;
				case 2:
					keyMgmt.RemoveSelfAndChildren(tier2Key);
					break;
				case 3:
					keyMgmt.RemoveSelfAndChildren(tier3Key);
					break;
			}
			// get all keys again
			allKeys = keyMgmt.FindKeys("*");
			Assert.NotEmpty(allKeys);
			if (tier == 1) {
				// all keys should be done
				Assert.Empty(allKeys.Where(x=>x.StartsWith(tier1Key.Key)));
			} else if (tier == 2) {
				Assert.Empty(allKeys.Where(x=>x.StartsWith(tier2Key.Key)));
			} else if (tier == 3) {
				Assert.Empty(allKeys.Where(x=>x.StartsWith(tier3Key.Key)));
			}
		}
	}
}
