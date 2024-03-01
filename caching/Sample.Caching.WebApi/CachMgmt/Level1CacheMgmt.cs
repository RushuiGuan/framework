using Albatross.Caching;
using Polly.Caching;
using Polly.Registry;

namespace Sample.Caching.WebApi {
	public class Level1CacheMgmt : CacheManagement<int, string> {
		public Level1CacheMgmt(ILogger<Level1CacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override string KeyPrefix => "k1";
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(5));

		public override void BuildKey(KeyBuilder builder, string _) {
			builder.Add(this, string.Empty);
		}
	}
}
