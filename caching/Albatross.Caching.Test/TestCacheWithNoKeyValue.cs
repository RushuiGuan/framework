using Sample.Caching.WebApi;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestCacheWithNoKeyValue {
		[Fact]
		public void TestKeyGeneration() {
			using var host = "redis".GetTestHost();
			using var scope = host.Create();
			var tier1 = scope.ServiceProvider.GetRequiredService<Level1CacheMgmt>();
			var tier2 = scope.ServiceProvider.GetRequiredService<Level2CacheMgmt>();
			var tier3 = scope.ServiceProvider.GetRequiredService<Level3CacheMgmt>();

			Assert.Equal("k1:", tier1.CreateKey(string.Empty));
			Assert.Equal("k1:*", tier1.CreateKey(string.Empty, true));
			Assert.Equal("k1:k2:", tier2.CreateKey(string.Empty));
			Assert.Equal("k1:k2:*", tier2.CreateKey(string.Empty, true));
			Assert.Equal("k1:k2:k3:", tier3.CreateKey(string.Empty));
			Assert.Equal("k1:k2:k3:*", tier3.CreateKey(string.Empty, true));
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestBasicOperation(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var tier1 = scope.ServiceProvider.GetRequiredService<Level1CacheMgmt>();

			await tier1.PutAsync(string.Empty, 1);

			var result = await tier1.TryGetAsync(string.Empty);
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
			var tier1 = scope.ServiceProvider.GetRequiredService<Level1CacheMgmt>();
			var tier2 = scope.ServiceProvider.GetRequiredService<Level2CacheMgmt>();
			var tier3 = scope.ServiceProvider.GetRequiredService<Level3CacheMgmt>();
			tier1.Reset();
			var keys = new List<string>();
			await tier1.PutAsync(string.Empty, 1);
			await tier2.PutAsync(string.Empty, 2);
			await tier3.PutAsync(string.Empty, 3);

			// base verification that all keys are created
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var item in keys) {
				Assert.Contains(tier1.CreateKey(item), allKeys);
				Assert.Contains(tier2.CreateKey(item), allKeys);
				Assert.Contains(tier3.CreateKey(item), allKeys);
			}
			tier3.Remove(string.Empty);
			allKeys = keyMgmt.FindKeys("*");
			Assert.DoesNotContain(tier3.CreateKey(string.Empty), allKeys);
			Assert.Contains(tier2.CreateKey(string.Empty), allKeys);
			Assert.Contains(tier1.CreateKey(string.Empty), allKeys);

			tier2.Remove(string.Empty);
			allKeys = keyMgmt.FindKeys("*");
			Assert.DoesNotContain(tier2.CreateKey(string.Empty), allKeys);
			Assert.Contains(tier1.CreateKey(string.Empty), allKeys);

			tier1.Remove(string.Empty);
			allKeys = keyMgmt.FindKeys("*");
			Assert.DoesNotContain(tier1.CreateKey(string.Empty), allKeys);
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
			var tier1 = scope.ServiceProvider.GetRequiredService<Level1CacheMgmt>();
			var tier2 = scope.ServiceProvider.GetRequiredService<Level2CacheMgmt>();
			var tier3 = scope.ServiceProvider.GetRequiredService<Level3CacheMgmt>();

			tier1.Reset();
			
			await tier1.PutAsync(string.Empty, 1);
			await tier2.PutAsync(string.Empty, 1);
			await tier3.PutAsync(string.Empty, 1);

			// base verification that all keys are created
			var allKeys = keyMgmt.FindKeys("*");
			Assert.Contains(tier1.CreateKey(string.Empty), allKeys);
			Assert.Contains(tier2.CreateKey(string.Empty), allKeys);
			Assert.Contains(tier3.CreateKey(string.Empty), allKeys);

			// remove the target key from the selected tier
			switch (tier) {
				case 1:
					tier1.RemoveSelfAndChildren(string.Empty);
					break;
				case 2:
					tier2.RemoveSelfAndChildren(string.Empty);
					break;
				case 3:
					tier3.RemoveSelfAndChildren(string.Empty);
					break;
			}
			// get all keys again
			allKeys = keyMgmt.FindKeys("*");
			if (tier == 1) {
				Assert.DoesNotContain(tier1.CreateKey(string.Empty), allKeys);
				Assert.DoesNotContain(tier2.CreateKey(string.Empty), allKeys);
				Assert.DoesNotContain(tier3.CreateKey(string.Empty), allKeys);
			} else if (tier == 2) {
				Assert.Contains(tier1.CreateKey(string.Empty), allKeys);
				Assert.DoesNotContain(tier2.CreateKey(string.Empty), allKeys);
				Assert.DoesNotContain(tier3.CreateKey(string.Empty), allKeys);
			} else if (tier == 3) {
				Assert.Contains(tier1.CreateKey(string.Empty), allKeys);
				Assert.Contains(tier2.CreateKey(string.Empty), allKeys);
				Assert.DoesNotContain(tier3.CreateKey(string.Empty), allKeys);
			}
		}
	}
}
