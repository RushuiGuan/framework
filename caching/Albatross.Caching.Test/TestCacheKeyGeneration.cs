using Albatross.Caching.Test.CacheMgmt;
using Albatross.Hosting.Test;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestCacheKeyGeneration {
		[Theory]
		[InlineData(MemCacheHost.HostType, "red", "stringkeycachemgmt:red:")]
		[InlineData(MemCacheHost.HostType, "test", "stringkeycachemgmt:test:")]
		public void TestKeyGeneration_StringKeyCacheMgmt(string hostType, string key, string expected) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var cache = scope.Get<StringKeyCacheMgmt>();
			var fullKey = cache.CreateKey(key, false);
			Assert.Equal(expected, fullKey);
			if (string.IsNullOrEmpty(key)) {
				fullKey = cache.CreateKey(string.Empty);
				Assert.Equal(expected, fullKey);
			}
		}

		[Theory]
		[InlineData(MemCacheHost.HostType, "", "slidingttlcachemgmt:")]
		[InlineData(MemCacheHost.HostType, null, "slidingttlcachemgmt:")]
		[InlineData(MemCacheHost.HostType, "red", "slidingttlcachemgmt:red:")]
		public void TestKeyGeneration_SlidingTtlCacheMgmt(string hostType, string? key, string expected) {
			using var host = hostType.GetTestHost();
			using var scope = host.Create();
			var cache = scope.Get<SlidingTtlCacheMgmt>();

#pragma warning disable CS8604 // suppress for testing reason
			var fullKey = cache.CreateKey(key);
#pragma warning restore CS8604 //

			Assert.Equal(expected, fullKey);
			if (string.IsNullOrEmpty(key)) {
				fullKey = cache.CreateKey(string.Empty);
				Assert.Equal(expected, fullKey);
			}
		}
	}
}