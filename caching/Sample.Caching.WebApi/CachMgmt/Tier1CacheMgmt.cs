using Albatross.Caching;
using Polly.Caching;
using Polly.Registry;

namespace Sample.Caching.WebApi {
	public class Tier1CacheMgmt : CacheManagement<string, MultiTierKey> {
		public Tier1CacheMgmt(ILogger<Tier1CacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override string KeyPrefix => "t1";
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(5));

		public override void BuildKey(KeyBuilder builder, MultiTierKey key) {
			builder.Add(this, key.Tier1);
		}
	}
}
