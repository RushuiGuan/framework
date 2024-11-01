using Albatross.Caching.BuiltIn;
using Albatross.Caching.Test.CacheKeys;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestParallelBehavior {
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
		[InlineData(My.MemCacheHostType, 3, 3)]
		public async Task TestParallelBahavior(string hostType, int expected_cacheMiss, int loopCount) {
			using var host = My.GetTestHost(hostType);
			using var scope = host.Services.CreateScope();
			var cache = scope.ServiceProvider.GetRequiredService<OneDayCache<MyData, CacheKey>>();
			string key = Guid.NewGuid().ToString();
			int cache_miss = 0;
			List<Task> tasks = new List<Task>();
			for (int i = 0; i < loopCount; i++) {
				var task = cache.ExecuteAsync(async context => {
					cache_miss++;
					await Task.Delay(2000);
					return new MyData();
				}, new CacheKey(key, true));
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
		[InlineData(My.MemCacheHostType, 1, 3)]
		public async Task TestParallelBahaviorWithLazy(string hostType, int expected_cacheMiss, int loopCount) {
			using var host = My.GetTestHost(hostType);
			using var scope = host.Services.CreateScope();
			var cacheMgmt = scope.ServiceProvider.GetRequiredService<OneDayCache<Lazy<Task<MyData>>, CacheKey>>();
			string key = Guid.NewGuid().ToString();
			int cache_miss = 0;
			var tasks = new List<Task<Lazy<Task<MyData>>>>();
			for (int i = 0; i < loopCount; i++) {
				var task = cacheMgmt.ExecuteAsync(context => Task.FromResult(new Lazy<Task<MyData>>(async () => {
					cache_miss++;
					await Task.Delay(2000);
					return new MyData();
				})), new CacheKey(key, true));
				tasks.Add(task);
			}
			var results = await Task.WhenAll(tasks);
			await Task.WhenAll(results.Select(args => args.Value));
			Assert.Equal(expected_cacheMiss, cache_miss);
		}
	}
}