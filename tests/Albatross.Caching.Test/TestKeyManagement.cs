using Albatross.Hosting.Test;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Polly;
using System;
using System.Linq;
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
			var factory = scope.Get<ICacheManagementFactory>();
			var cache = factory.Get<MyData>(nameof(LongTermCacheMgmt1));
			var keyMgmt = scope.Get<ICacheKeyManagement>();

			var data = new MyData("a");
			var keys = new string[] { "", "1", "2", "3" };

			foreach(var key in keys) { 
				await cache.PutAsync(new Polly.Context(key), data);
			}

			var allKeys = keyMgmt.FindKeys("*");


			foreach(var key in keys) {
				Assert.Contains(cache.GetCacheKey(new Context(key)), allKeys);
			}
		}
	}
}