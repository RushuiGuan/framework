﻿using Albatross.Caching.BuiltIn;
using Albatross.Caching.Test.CacheKeys;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class MyCacheKey : CacheKey {
		public MyCacheKey(string? value) : base("my-cache", value, true) {
		}
	}

	public class TestCacheKeyReset {
		[Theory]
		[InlineData(My.MemCacheHostType)]
		[InlineData(My.RedisCacheHostType)]
		public async Task TestResetOperation(string hostType) {
			using var host = My.GetTestHost(hostType);
			using var scope = host.Services.CreateScope();
			var keyMgmt = scope.ServiceProvider.GetRequiredService<ICacheKeyManagement>();
			var cache = scope.ServiceProvider.GetRequiredService<OneDayCache<MyData, MyCacheKey>>();
			keyMgmt.Reset(new MyCacheKey(null));

			var keys = new List<MyCacheKey>();
			for (int x = 0; x < 10; x++) {
				var cacheKey = new MyCacheKey(x.ToString());
				keys.Add(cacheKey);
				await cache.PutAsync(cacheKey, new MyData());
			}

			// base verification that all keys are created
			var allKeys = keyMgmt.FindKeys(new MyCacheKey(null).WildCardKey);
			foreach (var item in keys) {
				Assert.Contains(item.Key, allKeys);
			}
			// reset the key
			keyMgmt.Reset(new MyCacheKey(null));
			allKeys = keyMgmt.FindKeys(new MyCacheKey(null).WildCardKey);
			Assert.Empty(allKeys);
		}
	}
}