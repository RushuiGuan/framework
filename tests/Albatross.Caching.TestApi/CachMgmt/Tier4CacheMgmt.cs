using Polly.Caching;
using Polly.Registry;

namespace Albatross.Caching.TestApi {
	public class Tier4CacheMgmt : CacheManagement<HisHerData> {
		private readonly Tier1CacheMgmt tier1;

		public Tier4CacheMgmt(Tier1CacheMgmt tier1, ILogger<Tier3CacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
			this.tier1 = tier1;
		}
		public override string KeyPrefix => "t4";
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(5));
	}
}
