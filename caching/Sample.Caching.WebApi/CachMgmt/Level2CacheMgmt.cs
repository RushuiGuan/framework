using Albatross.Caching;
using Polly.Caching;
using Polly.Registry;

namespace Sample.Caching.WebApi {
	public class Level2CacheMgmt : CacheManagement<int, string> {
		private readonly Level1CacheMgmt parent;

		public Level2CacheMgmt(Level1CacheMgmt parent, ILogger<Level2CacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
			this.parent = parent;
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(5));
		public override string KeyPrefix => "k2";
		public override void BuildKey(KeyBuilder builder, string key) {
			parent.BuildKey(builder, key);
			builder.Add(this, string.Empty);
		}
	}
}
