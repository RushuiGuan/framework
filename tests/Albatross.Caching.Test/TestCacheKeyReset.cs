using Albatross.Hosting.Test;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using Albatross.Caching.Test.CacheMgmt;

namespace Albatross.Caching.Test {
	public class TestCacheKeyReset {
		[Theory]
		[InlineData(MemCacheHost.HostType)]
		[InlineData(RedisCacheHost.HostType)]
		public async Task TestResetOperation(string hostType) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var keyMgmt = scope.Get<ICacheKeyManagement>();
			var cache = scope.ServiceProvider.GetRequiredService<StringKeyCacheMgmt>();
			keyMgmt.Remove("*");

			var keys = new List<string>();
			for (int x = 0; x < 10; x++) {
				keys.Add(x.ToString());
				await cache.PutAsync(x.ToString(), new MyData());
			}

			// base verification that all keys are created
			var allKeys = keyMgmt.FindKeys("*");
			foreach (var item in keys) {
				Assert.Contains(cache.CreateKey(item), allKeys);
			}
			// reset the key
			cache.Reset();
			allKeys = keyMgmt.FindKeys("*");
			foreach (var item in keys) {
				Assert.DoesNotContain(cache.CreateKey(item), allKeys);
			}
		}
	}
}
