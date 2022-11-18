using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestEviction: IClassFixture<MyTestHost>{
		private readonly MyTestHost host;

		public TestEviction(MyTestHost host) {
			this.host = host;
		}

		[Fact]
		public void TestReset() {
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
		public async Task TestCacheMgmtEvictSingle() {
			var cacheMgmt = this.host.CacheFactory.Get<string>(nameof(MyCacheMgmt));
			var cacheMgmt1 = this.host.CacheFactory.Get<string>(nameof(MyCacheMgmt1));
			await cacheMgmt.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context("a"));
			await cacheMgmt.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context("b"));
			await cacheMgmt.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context());

			await cacheMgmt1.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context("a"));
			await cacheMgmt1.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context("b"));
			await cacheMgmt1.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context());

			Assert.Collection(this.host.CacheExtended.Keys.OrderBy(args => args),
				args => Assert.Equal($"{nameof(MyCacheMgmt).ToLower()}-", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt).ToLower()}-a", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt).ToLower()}-b", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-a", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-b", args)
			);
			cacheMgmt.Evict(new Polly.Context());
			Assert.Collection(this.host.CacheExtended.Keys.OrderBy(args => args),
				args => Assert.Equal($"{nameof(MyCacheMgmt).ToLower()}-a", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt).ToLower()}-b", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-a", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-b", args)
			);
		}


		[Fact]
		public async Task TestCacheMgmtEvictAll() {
			var cacheMgmt = this.host.CacheFactory.Get<string>(nameof(MyCacheMgmt));
			var cacheMgmt1 = this.host.CacheFactory.Get<string>(nameof(MyCacheMgmt1));
			await cacheMgmt.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context("a"));
			await cacheMgmt.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context("b"));
			await cacheMgmt.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context());

			await cacheMgmt1.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context("a"));
			await cacheMgmt1.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context("b"));
			await cacheMgmt1.ExecuteAsync(context => Task.FromResult(context.OperationKey), new Polly.Context());

			Assert.Collection(this.host.CacheExtended.Keys.OrderBy(args => args),
				args => Assert.Equal($"{nameof(MyCacheMgmt).ToLower()}-", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt).ToLower()}-a", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt).ToLower()}-b", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-a", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-b", args)
			);
			cacheMgmt.EvictAll();
			Assert.Collection(this.host.CacheExtended.Keys.OrderBy(args => args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-a", args),
				args => Assert.Equal($"{nameof(MyCacheMgmt1).ToLower()}-b", args)
			);
			cacheMgmt1.EvictAll();
			Assert.Empty(this.host.CacheExtended.Keys);
		}
	}
}
