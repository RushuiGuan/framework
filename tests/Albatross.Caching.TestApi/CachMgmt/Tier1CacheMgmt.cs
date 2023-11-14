using Polly.Caching;
using Polly.Registry;

namespace Albatross.Caching.TestApi {
	public class Tier1CacheMgmt : CacheManagement<string> {
		public Tier1CacheMgmt(ILogger<Tier1CacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override string KeyPrefix => "t1";
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(5));
	}
}
