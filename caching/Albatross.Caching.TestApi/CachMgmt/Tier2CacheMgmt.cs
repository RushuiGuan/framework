using Polly.Caching;
using Polly.Registry;
using System.Text;

namespace Albatross.Caching.TestApi {
	public class Tier2CacheMgmt : CacheManagement<string, MultiTierKey> {
		private readonly Tier1CacheMgmt parent;

		public Tier2CacheMgmt(Tier1CacheMgmt parent, ILogger<Tier2CacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
			this.parent = parent;
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(5));
		public override string KeyPrefix => "t2";
		public override void BuildKey(KeyBuilder builder, MultiTierKey key) {
			parent.BuildKey(builder, key);
			builder.Add(this, key.Tier2);
		}
	}
}
