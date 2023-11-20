using Polly.Caching;
using Polly.Registry;

namespace Albatross.Caching.TestApi {
	public class Tier3CacheMgmt : CacheManagement<string, MultiTierKey> {
		private readonly Tier2CacheMgmt parent;

		public Tier3CacheMgmt(Tier2CacheMgmt parent, ILogger<Tier3CacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
			this.parent = parent;
		}
		public override string KeyPrefix => "t3";
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(5));
		public override void BuildKey(KeyBuilder builder, MultiTierKey key) {
			parent.BuildKey(builder, key);
			builder.Add(this, key.Tier3);
		}
	}
}
