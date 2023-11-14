using Polly;
using Polly.Caching;
using Polly.Registry;

namespace Albatross.Caching.TestApi {
	public class HisHerData {
		public HisHerData(string name, int id) {
			Name = name;
			Id = id;
		}
		public string Name { get; set; }
		public int Id { get; set; }
	}

	public class Tier3CacheMgmt : CacheManagement<HisHerData> {
		private readonly Tier1CacheMgmt tier1;

		public Tier3CacheMgmt(Tier1CacheMgmt tier1, ILogger<Tier3CacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
			this.tier1 = tier1;
		}
		public override string KeyPrefix => "t3";
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(5));
	}
}
