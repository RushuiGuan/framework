using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Threading.Tasks;

namespace Albatross.Caching.Test.CacheMgmt {
	public class LazyCacheMgmt : CacheManagement<Lazy<Task<MyData>>, string> {
		public LazyCacheMgmt(ILogger<StringKeyCacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(1));
	}
}
