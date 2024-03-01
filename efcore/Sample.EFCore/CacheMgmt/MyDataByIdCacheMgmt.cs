using Albatross.Caching;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;

namespace Sample.EFCore.CacheMgmt {
	public class MyDataByIdCacheMgmt : Albatross.Caching.CacheManagement<MyDataDto, int> {
		public MyDataByIdCacheMgmt(ILogger logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProviderAdapter, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProviderAdapter, keyMgmt) {
		}

		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromHours(10));
	}

	public class MyDataDto {
	}
}
