using Albatross.Caching;
using Albatross.Caching.MemCache;
using Polly.Caching;
using Polly.Registry;

namespace Sample.Caching.WebApi.CachMgmt {
	public class MyMemCacheMgmt : CacheManagement<string, string> {
		public MyMemCacheMgmt(ILogger<MyMemCacheMgmt> logger, IPolicyRegistry<string> registry,
			ObjectCacheProviderAdapter cacheProviderAdapter, MemoryCacheKeyManagement keyMgmt) : base(logger, registry, cacheProviderAdapter, keyMgmt) {
		}

		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromSeconds(5));
	}
}
