﻿using Albatross.Hosting.Test;
using Polly;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestKeyManagement {
		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestKeyCreation(string hostType) {
			using var host = hostType.GetTestHost();
			var scope = host.Create();
			var cache = scope.Get<LongTermCacheMgmt1>();
			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var data = new MyData("a");
			var keys = new string[] { "", "1", "2", "3" };

			foreach(var key in keys) { 
				await cache.PutAsync(new object[] { key }, data);
			}
			var allKeys = keyMgmt.FindKeys("*");
			foreach(var key in keys) {
				Assert.Contains(cache.GetCacheKey(new Context(key)), allKeys);
			}
		}


		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestKeyRemoval(string hostType) {
			using var host = hostType.GetTestHost();
			var scope = host.Create();
			var cache = scope.Get<LongTermCacheMgmt1>();
			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var data = new MyData("a");
			var keys = new string[] { "", "1", "2", "3" };

			foreach (var key in keys) {
				await cache.PutAsync(new object[] { key }, data);
			}
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.Contains(cache.GetCacheKey(new Context(key)), allKeys);
			}
			foreach (var key in keys) {
				cache.Remove(key);
			}
			allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.DoesNotContain(cache.GetCacheKey(new Context(key)), allKeys);
			}
		}

		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestKeyReset(string hostType) {
			using var host = hostType.GetTestHost();
			var scope = host.Create();
			var cache1 = scope.Get<LongTermCacheMgmt1>();
			var cache2 = scope.Get<LongTermCacheMgmt2>(); 
			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var data = new MyData("a");
			var keys = new string[] { "", "1", "2", "3" };

			foreach (var key in keys) {
				await cache1.PutAsync(new object[]{ key }, data);
				await cache2.PutAsync(new object[]{ key }, data);
			}

			var allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.Contains(cache1.GetCacheKey(new Context(key)), allKeys);
			}
			cache1.RemoveAll();
			
			allKeys = keyMgmt.FindKeys("*");
			foreach (var key in keys) {
				Assert.DoesNotContain(cache1.GetCacheKey(new Context(key)), allKeys);
			}
			foreach (var key in keys) {
				Assert.Contains(cache2.GetCacheKey(new Context(key)), allKeys);
			}
		}
	}
}