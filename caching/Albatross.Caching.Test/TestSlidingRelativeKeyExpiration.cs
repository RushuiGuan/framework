using Albatross.Caching.Test.CacheMgmt;
using Albatross.Hosting.Test;
using Polly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestSlidingRelativeKeyExpiration {

		[Theory]
		[InlineData(MemCacheHost.HostType, 0, 1, 1)]
		[InlineData(MemCacheHost.HostType, 0, 10, 1)]
		[InlineData(MemCacheHost.HostType, 1200, 2, 2)]
		[InlineData(MemCacheHost.HostType, 200, 6, 2)]

		[InlineData(RedisCacheHost.HostType, 0, 1, 1)]
		[InlineData(RedisCacheHost.HostType, 0, 10, 1)]
		[InlineData(RedisCacheHost.HostType, 1200, 5, 5)]
		[InlineData(RedisCacheHost.HostType, 200, 6, 2)]

		/// <see cref="RelativeTtlCacheMgmt"/> has 1 second relative ttl.  This test use different combination of loop and delays to test out cache hit and miss count
		public async Task TestRelativeCacheManagement(string hostType, int delay_ms, int loopCount, int expected_cacheMiss) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var cacheMgmt = scope.Get<RelativeTtlCacheMgmt>();
			string key = Guid.NewGuid().ToString();
			int cache_miss = 0;
			for (int i = 0; i < loopCount; i++) {
				var result = await cacheMgmt.ExecuteAsync(context => {
					cache_miss++;
					return Task.FromResult(new object());
				}, key);
				await Task.Delay(delay_ms);
			}
			Assert.Equal(expected_cacheMiss, cache_miss);
		}

		[Theory]
		[InlineData(MemCacheHost.HostType, 0, 1, 1)]
		[InlineData(MemCacheHost.HostType, 0, 10, 1)]
		[InlineData(MemCacheHost.HostType, 800, 10, 1)]
		[InlineData(MemCacheHost.HostType, 200, 6, 1)]
		[InlineData(MemCacheHost.HostType, 1200, 2, 2)]
		[InlineData(RedisCacheHost.HostType, 0, 1, 1)]
		[InlineData(RedisCacheHost.HostType, 0, 10, 1)]
		[InlineData(RedisCacheHost.HostType, 800, 10, 1)]
		[InlineData(RedisCacheHost.HostType, 200, 6, 1)]
		[InlineData(RedisCacheHost.HostType, 1200, 2, 2)]
		/// <see cref="SlidingTtlCacheMgmt"/> has 1 second sliding ttl.  This test use different combination of loop and delays to test out cache hit and miss count
		public async Task TestSlidingCacheManagement(string hostType, int delay_ms, int loopCount, int expected_cacheMiss) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var cacheMgmt = scope.Get<SlidingTtlCacheMgmt>();
			string key = Guid.NewGuid().ToString();
			int cache_miss = 0;
			for (int i = 0; i < loopCount; i++) {
				var result = await cacheMgmt.ExecuteAsync(context => {
					cache_miss++;
					return Task.FromResult(new MyData());
				}, key);
				await Task.Delay(delay_ms);
			}
			Assert.Equal(expected_cacheMiss, cache_miss);
		}
	}
}