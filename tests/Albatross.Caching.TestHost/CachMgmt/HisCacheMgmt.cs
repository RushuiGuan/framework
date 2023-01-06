using Polly.Caching;
using Polly.Registry;

namespace Albatross.Caching.TestHost {
	public class HisHerData {
		public HisHerData(string name, int id) {
			Name = name;
			Id = id;
		}
		public string Name { get; set; }
		public int Id { get; set; }
	}

	public class HisCacheMgmt : CacheManagement<HisHerData> {
		public HisCacheMgmt(ILogger<HisCacheMgmt> logger, IPolicyRegistry<string> registry, IAsyncCacheProviderConverter cacheProvider, IRedisKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(5));
	}

	public class HerCacheMgmt : CacheManagement<HisHerData> {
		public HerCacheMgmt(ILogger<HisCacheMgmt> logger, IPolicyRegistry<string> registry, IAsyncCacheProviderConverter cacheProvider, IRedisKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(5));
	}
}
