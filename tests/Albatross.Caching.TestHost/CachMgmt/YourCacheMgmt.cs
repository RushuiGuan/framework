using Polly.Caching;
using Polly.Registry;

namespace Albatross.Caching.TestHost {
	public class YourCacheMgmt : CacheManagement<byte[]> {
		public YourCacheMgmt(ILogger<YourCacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(5));
	}
}
