using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Diagnostics.Metrics;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestCacheKeys : IClassFixture<MyTestHost>{
		private readonly MyTestHost host;

		public TestCacheKeys(MyTestHost host) {
			this.host = host;
		}
		[Fact]
		public void TestResetCache() {
			MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
			cache.GetOrCreate<string>(1, e => "a");
			cache.GetOrCreate<string>(2, e => "b");
			cache.GetOrCreate<string>(3, e => "c");
			Assert.True(cache.Count == 3);
			var reset = new MemoryCacheExtended(cache);
			reset.Reset();
			Assert.True(cache.Count == 0);
		}

		[Fact]
		public void TestAllKeys() {
			MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
			cache.GetOrCreate<string>(1, e => "a");
			cache.GetOrCreate<string>(2, e => "b");
			cache.GetOrCreate<string>(3, e => "c");
			Assert.True(cache.Count == 3);
			var extended = new MemoryCacheExtended(cache);
			Assert.Collection(extended.Keys,
				args => Assert.Equal(1, args),
				args => Assert.Equal(2, args),
				args => Assert.Equal(3, args)
			);
		}
		[Fact]
		public void TestNullOperationKey() {
			Context c = new Context();
			Assert.True(string.IsNullOrEmpty(c.OperationKey));
			Assert.Null(c.OperationKey);
		}

		[Fact]
		public void TestEmptyOperationKey() {
			Context c = new Context(string.Empty);
			Assert.Equal(string.Empty, c.OperationKey);
		}

		[Theory]
		[InlineData("mycachemgmt-", "")]
		[InlineData("mycachemgmt-", null)]
		[InlineData("mycachemgmt-a", "a")]
		[InlineData("mycachemgmt-1", "1")]
		public void TestCacheManagementKeys(string expected, string key) {
			var scope = host.Create();
			var factory = scope.Provider.GetRequiredService<ICacheManagementFactory>();
			var cacheMgmt = factory.Get<string>(nameof(MyCacheMgmt));
			string result = cacheMgmt.GetCacheKey(new Context(key));
			Assert.Equal(expected, result);
		}
	}
}
