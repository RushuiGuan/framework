using Albatross.Caching.Test.CacheKeys;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestSlidingRelativeKeyExpiration {

		[Theory]
		[InlineData(My.MemCacheHostType, 0, 1, 1)]
		[InlineData(My.MemCacheHostType, 0, 10, 1)]
		[InlineData(My.MemCacheHostType, 1200, 2, 2)]
		[InlineData(My.MemCacheHostType, 200, 6, 2)]

		[InlineData(My.RedisCacheHostType, 0, 1, 1)]
		[InlineData(My.RedisCacheHostType, 0, 10, 1)]
		[InlineData(My.RedisCacheHostType, 1200, 5, 5)]
		[InlineData(My.RedisCacheHostType, 200, 6, 2)]

		/// <see cref="RelativeTtlCacheMgmt"/> has 1 second relative ttl.  This test use different combination of loop and delays to test out cache hit and miss count
		public async Task TestRelativeCacheManagement(string hostType, int delay_ms, int loopCount, int expected_cacheMiss) {
			using var host = My.GetTestHost(hostType);
			using var scope = host.Services.CreateScope();
			var cache = scope.ServiceProvider.GetRequiredService<BuiltIn.OneSecondCache<object, CacheKey>>();
			string keyValue = Guid.NewGuid().ToString();
			int cache_miss = 0;
			for (int i = 0; i < loopCount; i++) {
				var result = await cache.ExecuteAsync(context => {
					cache_miss++;
					return Task.FromResult(new object());
				}, new CacheKey(keyValue, true));
				await Task.Delay(delay_ms);
			}
			Assert.Equal(expected_cacheMiss, cache_miss);
		}

		[Theory]
		[InlineData(My.MemCacheHostType, 0, 1, 1)]
		[InlineData(My.MemCacheHostType, 0, 10, 1)]
		[InlineData(My.MemCacheHostType, 800, 10, 1)]
		[InlineData(My.MemCacheHostType, 200, 6, 1)]
		[InlineData(My.MemCacheHostType, 100, 10, 1)]
		[InlineData(My.MemCacheHostType, 1200, 2, 2)]
		[InlineData(My.RedisCacheHostType, 0, 1, 1)]
		[InlineData(My.RedisCacheHostType, 0, 10, 1)]
		[InlineData(My.RedisCacheHostType, 800, 10, 1)]
		[InlineData(My.RedisCacheHostType, 200, 6, 1)]
		[InlineData(My.RedisCacheHostType, 1200, 2, 2)]
		/// <see cref="SlidingTtlCacheMgmt"/> has 1 second sliding ttl.  This test use different combination of loop and delays to test out cache hit and miss count
		public async Task TestSlidingCacheManagement(string hostType, int delay_ms, int loopCount, int expected_cacheMiss) {
			using var host = My.GetTestHost(hostType);
			using var scope = host.Services.CreateScope();
			var cacheMgmt = scope.ServiceProvider.GetRequiredService<BuiltIn.OneSecondSlidingTtlCache<MyData, CacheKey>>();
			string keyValue = Guid.NewGuid().ToString();
			int cache_miss = 0;
			for (int i = 0; i < loopCount; i++) {
				var result = await cacheMgmt.ExecuteAsync(context => {
					cache_miss++;
					return Task.FromResult(new MyData());
				}, new CacheKey(keyValue, true));
				await Task.Delay(delay_ms);
			}
			Assert.Equal(expected_cacheMiss, cache_miss);
		}
	}
}