using Polly.Caching;
using Polly.Registry;

namespace Albatross.Caching.TestApi {
	public class Level3CacheMgmt : CacheManagement<int, string> {
		private readonly Level2CacheMgmt parent;

		public Level3CacheMgmt(Level2CacheMgmt parent, ILogger<Level3CacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
			this.parent = parent;
		}
		public override string KeyPrefix => "k3";
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(5));
		public override void BuildKey(KeyBuilder builder, string key) {
			parent.BuildKey(builder, key);
			builder.Add(this, string.Empty);
		}
	}
}
