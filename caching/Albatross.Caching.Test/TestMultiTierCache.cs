using Albatross.Hosting.Test;
using Albatross.Caching.TestApi;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;

namespace Albatross.Caching.Test {
	public class TestMultiTierCache {
		[Fact]
		public void TestKeyGeneration() {
			using var host = "redis".GetTestHost();
			using var scope = host.Create();
			var tier1 = scope.ServiceProvider.GetRequiredService<Tier1CacheMgmt>();
			var tier2 = scope.ServiceProvider.GetRequiredService<Tier2CacheMgmt>();
			var tier3 = scope.ServiceProvider.GetRequiredService<Tier3CacheMgmt>();

			var keys = new MultiTierKey[] {
				new MultiTierKey(1, 2, 3),
				new MultiTierKey(11, 22, 33),
			};

			Assert.Equal("t1:1:", tier1.CreateKey(new MultiTierKey(1, 2, 3)));
			Assert.Equal("t1:1:*", tier1.CreateKey(new MultiTierKey(1, 2, 3), true));
			Assert.Equal("t1:1:t2:2:", tier2.CreateKey(new MultiTierKey(1, 2, 3)));
			Assert.Equal("t1:1:t2:2:*", tier2.CreateKey(new MultiTierKey(1, 2, 3), true));
			Assert.Equal("t1:1:t2:2:t3:3:", tier3.CreateKey(new MultiTierKey(1, 2, 3)));
			Assert.Equal("t1:1:t2:2:t3:3:*", tier3.CreateKey(new MultiTierKey(1, 2, 3), true));
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestBasicOperation(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var tier1 = scope.ServiceProvider.GetRequiredService<Tier1CacheMgmt>();

			var keys = new MultiTierKey[] {
				new MultiTierKey(1),
				new MultiTierKey(2),
				new MultiTierKey(3),
				new MultiTierKey(4),
				new MultiTierKey(5),
			};

			foreach (var key in keys) {
				await tier1.PutAsync(key, key.Data.ToString());
			}

			foreach (var key in keys) {
				var result = await tier1.TryGetAsync(key, CancellationToken.None);
				Assert.True(result.Item1);
				Assert.Equal(key.Data.ToString(), result.Item2);
			}
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestTieredKeyRemoveSelfOnly(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var keyMgmt = scope.Get<ICacheKeyManagement>();
			var tier1 = scope.ServiceProvider.GetRequiredService<Tier1CacheMgmt>();
			var tier2 = scope.ServiceProvider.GetRequiredService<Tier2CacheMgmt>();
			var tier3 = scope.ServiceProvider.GetRequiredService<Tier3CacheMgmt>();

			tier1.Reset();
			
			var keys = new List<MultiTierKey>();
			for (int x = 0; x < 3; x++) {
				for (int y = 0; y < 3; y++) {
					for (int z = 0; z < 3; z++) {
						var key = new MultiTierKey(x, y, z);
						keys.Add(key);
						await tier1.PutAsync(key, key.Data.ToString());
						await tier2.PutAsync(key, key.Data.ToString());
						await tier3.PutAsync(key, key.Data.ToString());
					}
				}
			}

			// base verification that all keys are created
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var item in keys) {
				Assert.Contains(tier1.CreateKey(item), allKeys);
				Assert.Contains(tier2.CreateKey(item), allKeys);
				Assert.Contains(tier3.CreateKey(item), allKeys);
			}
			// remove one key at a time and make sure only that key is removed
			foreach (var item in keys) {
				tier1.Remove(item);
				tier2.Remove(item);
				tier3.Remove(item);
				allKeys = keyMgmt.FindKeys("*");
				Assert.DoesNotContain(tier1.CreateKey(item), allKeys);
				Assert.DoesNotContain(tier2.CreateKey(item), allKeys);
				Assert.DoesNotContain(tier3.CreateKey(item), allKeys);
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
		public async Task TestTieredKeyRemoveSelfAndChildren(string hostType, int tier, int key1, int key2, int key3) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var keyMgmt = scope.Get<ICacheKeyManagement>();
			var tier1 = scope.ServiceProvider.GetRequiredService<Tier1CacheMgmt>();
			var tier2 = scope.ServiceProvider.GetRequiredService<Tier2CacheMgmt>();
			var tier3 = scope.ServiceProvider.GetRequiredService<Tier3CacheMgmt>();

			tier1.Reset();

			var keys = new List<MultiTierKey>();
			for (int x = 0; x < 3; x++) {
				for (int y = 0; y < 3; y++) {
					for (int z = 0; z < 3; z++) {
						var key = new MultiTierKey(x, y, z);
						keys.Add(key);
						await tier1.PutAsync(key, key.Data.ToString());
						await tier2.PutAsync(key, key.Data.ToString());
						await tier3.PutAsync(key, key.Data.ToString());
					}
				}
			}
			// base verification that all keys are created
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var item in keys) {
				Assert.Contains(tier1.CreateKey(item), allKeys);
				Assert.Contains(tier2.CreateKey(item), allKeys);
				Assert.Contains(tier3.CreateKey(item), allKeys);
			}
			// remove the target key from the selected tier
			var targetKey = new MultiTierKey(key1, key2, key3);
			switch (tier) {
				case 1:
					tier1.RemoveSelfAndChildren(targetKey);
					break;
				case 2:
					tier2.RemoveSelfAndChildren(targetKey);
					break;
				case 3:
					tier3.RemoveSelfAndChildren(targetKey);
					break;
			}
			// get all keys again
			allKeys = keyMgmt.FindKeys("*");
			foreach (var item in keys) {
				if (tier == 1 && item.Tier1 == key1) {
					Assert.DoesNotContain(tier1.CreateKey(item), allKeys);
					Assert.DoesNotContain(tier2.CreateKey(item), allKeys);
					Assert.DoesNotContain(tier3.CreateKey(item), allKeys);
				} else if (tier == 2 && item.Tier1 == key1 && item.Tier2 == key2) {
					Assert.Contains(tier1.CreateKey(item), allKeys);
					Assert.DoesNotContain(tier2.CreateKey(item), allKeys);
					Assert.DoesNotContain(tier3.CreateKey(item), allKeys);
				} else if(tier == 3 && item.Tier1 == key1 && item.Tier2 == key2 && item.Tier3 == key3) {
					Assert.Contains(tier1.CreateKey(item), allKeys);
					Assert.Contains(tier2.CreateKey(item), allKeys);
					Assert.DoesNotContain(tier3.CreateKey(item), allKeys);
				} else {
					Assert.Contains(tier1.CreateKey(item), allKeys);
					Assert.Contains(tier2.CreateKey(item), allKeys);
					Assert.Contains(tier3.CreateKey(item), allKeys);
				}
			}
		}
	}
}
