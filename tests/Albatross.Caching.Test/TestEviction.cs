using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestEviction: IClassFixture<MyTestHost>{
		private readonly MyTestHost host;

		public TestEviction(MyTestHost host) {
			this.host = host;
		}

		[Fact]
		public void TestMultipleRegistrations() {
			var scope = host.Create();
			var items = scope.Get<IEnumerable<ICacheManagement>>();
			Assert.Equal(2, items.Count());
		}

		[Fact]
		public void TestSingletonRegistrationAgainstCollectionRegistration() {
			var scope = host.Create();
			var items = scope.Get<IEnumerable<ICacheManagement>>();
			var single = scope.Get<ICacheManagement>();
			Assert.NotSame(single, items.First());
			Assert.Same(single, items.Last());
		}

		[Fact]
		public void TestReset() {
			var scope = host.Create();

			ILogger<MemoryCacheExtended> logger = new Mock<ILogger<MemoryCacheExtended>>().Object;
			IHttpClientFactory factory = new Mock<IHttpClientFactory>().Object;
			MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
			cache.GetOrCreate<string>(1, e => "a");
			cache.GetOrCreate<string>(2, e => "b");
			cache.GetOrCreate<string>(3, e => "c");
			Assert.True(cache.Count == 3);
			var reset = new MemoryCacheExtended(cache, factory, new CachingConfig(new Mock<IConfiguration>().Object), logger);
			reset.Reset();
			Assert.True(cache.Count == 0);
		}
	}
}
