using Polly;
using Polly.Caching;
using Polly.Registry;

namespace Albatross.Caching.TestApi {
	public class Tier2CacheMgmt : CacheManagement<byte[]> {
		private readonly Tier2CacheMgmt parent;

		public Tier2CacheMgmt(Tier2CacheMgmt parent, ILogger<Tier2CacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
			this.parent = parent;
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(5));
		public override string KeyPrefix => "t2";

		public override string BuildKey(params object[] compositeKey) {

			return base.BuildKey(compositeKey);
		}
	}
}
