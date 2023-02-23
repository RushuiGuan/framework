using Albatross.Hosting.Test;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestCaching {

		[Theory]
		[InlineData(MemCacheHost.HostType, 0, 1, 1)]
		[InlineData(MemCacheHost.HostType, 0, 10, 1)]
		[InlineData(MemCacheHost.HostType, 1200, 2, 2)]
		[InlineData(MemCacheHost.HostType, 200, 6, 2)]

		[InlineData(RedisCacheHost.HostType, 0, 1, 1)]
		[InlineData(RedisCacheHost.HostType, 0, 10, 1)]
		[InlineData(RedisCacheHost.HostType, 1200, 2, 2)]
		[InlineData(RedisCacheHost.HostType, 200, 6, 2)]

		/// <see cref="RelativeTtlCacheMgmt"/> has 1 second relative ttl.  This test use different combination of loop and delays to test out cache hit and miss count
		public async Task TestRelativeCacheManagement(string hostType, int delay_ms, int loopCount, int expected_cacheMiss) {
			using var host = hostType.GetTestHost();
			var scope = host.Create();
			var factory = scope.Get<ICacheManagementFactory>();
			var cacheMgmt = factory.Get<object>(nameof(RelativeTtlCacheMgmt));
			string key = Guid.NewGuid().ToString();
			int cache_miss = 0;
			for (int i = 0; i < loopCount; i++) {
				var result = await cacheMgmt.ExecuteAsync(context => {
					cache_miss++;
					return Task.FromResult(new object());
				}, new Context(key));
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
			var scope = host.Create();
			var factory = scope.Get<ICacheManagementFactory>();
			var cacheMgmt = factory.Get<MyData>(nameof(SlidingTtlCacheMgmt));
			string key = Guid.NewGuid().ToString();
			int cache_miss = 0;
			for (int i = 0; i < loopCount; i++) {
				var result = await cacheMgmt.ExecuteAsync(context => {
					cache_miss++;
					return Task.FromResult(new MyData());
				}, new Context(key));
				await Task.Delay(delay_ms);
			}
			Assert.Equal(expected_cacheMiss, cache_miss);
		}


		[Theory]
		[InlineData(MemCacheHost.HostType, nameof(SlidingTtlCacheMgmt), "", "slidingttlcachemgmt:")]
		[InlineData(MemCacheHost.HostType, nameof(SlidingTtlCacheMgmt), null, "slidingttlcachemgmt:")]
		[InlineData(MemCacheHost.HostType, nameof(SlidingTtlCacheMgmt), "red", "slidingttlcachemgmt:red")]
		[InlineData(MemCacheHost.HostType, nameof(LongTermCacheMgmt1), "red", "longtermcachemgmt1:red")]
		[InlineData(MemCacheHost.HostType, nameof(LongTermCacheMgmt1), "test", "longtermcachemgmt1:test")]
		public void TestCacheKeyGeneration(string hostType, string name, string key, string expected) {
			using var host = hostType.GetTestHost();
			var scope = host.Create();
			var factory = scope.Get<ICacheManagementFactory>();
			var cache = factory.Get<MyData>(name);
			var fullKey = cache.GetCacheKey(new Context(key));
			Assert.Equal(expected, fullKey);
			if (string.IsNullOrEmpty(key)) {
				fullKey = cache.GetCacheKey(new Context());
				Assert.Equal(expected, fullKey);
			}
		}

		/// <summary>
		/// This test is to prove the behavior of caching.  When multiple caching calls of the same key are requested
		/// at the same time, polly will execute them at the same time.  Polly will not execute the first request and
		/// use the cached result for the rest of the request.  This is by design and the main reason is simplicity.
		/// To go around this, we can try mutex or a lazy object tested below.  Neither is a good solution.  We should
		/// stick with the simple solution, but change the behavior of caching.
		/// </summary>
		/// <param name="hostType"></param>
		/// <param name="expected_cacheMiss"></param>
		/// <param name="loopCount"></param>
		/// <returns></returns>
		[Theory]
		[InlineData(MemCacheHost.HostType, 3, 3)]
		public async Task TestParallelBahavior(string hostType, int expected_cacheMiss, int loopCount) {
			using var host = hostType.GetTestHost();
			var scope = host.Create();
			var factory = scope.Get<ICacheManagementFactory>();
			var cacheMgmt = factory.Get<MyData>(nameof(LongTermCacheMgmt1));
			string key = Guid.NewGuid().ToString();
			int cache_miss = 0;
			List<Task> tasks = new List<Task>();
			for (int i = 0; i < loopCount; i++) {
				var task = cacheMgmt.ExecuteAsync(async context => {
					cache_miss++;
					await Task.Delay(2000);
					return new MyData();
				}, new Context(key));
				tasks.Add(task);
			}
			await Task.WhenAll(tasks);
			Assert.Equal(expected_cacheMiss, cache_miss);
		}

		/// <summary>
		/// use of Lazy with caching is generally not a good idea.  it will only work with in memeory caching.
		/// we cannot serialize a lazy object into bytes.  it also will move the exception handling call stack
		/// outside of the caching call stack.  any retry attempt will not work since a lazy object will always
		/// be successfully returned.
		/// </summary>
		/// <param name="hostType"></param>
		/// <param name="expected_cacheMiss"></param>
		/// <param name="loopCount"></param>
		/// <returns></returns>
		[Theory]
		[InlineData(MemCacheHost.HostType, 1, 3)]
		public async Task TestParallelBahaviorWithLazy(string hostType, int expected_cacheMiss, int loopCount) {
			using var host = hostType.GetTestHost();
			var scope = host.Create();
			var factory = scope.Get<ICacheManagementFactory>();
			var cacheMgmt = factory.Get<Lazy<Task<MyData>>>(nameof(LongTermCacheMgmt4));
			string key = Guid.NewGuid().ToString();
			int cache_miss = 0;
			var tasks = new List<Task<Lazy<Task<MyData>>>>();
			for (int i = 0; i < loopCount; i++) {
				var task = cacheMgmt.ExecuteAsync(context => Task.FromResult(new Lazy<Task<MyData>>(async () => {
					cache_miss++;
					await Task.Delay(2000);
					return new MyData();
				})), new Context(key));
				tasks.Add(task);
			}
			var results = await Task.WhenAll(tasks);
			await Task.WhenAll(results.Select(args => args.Value));
			Assert.Equal(expected_cacheMiss, cache_miss);
		}
	}
}