using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching.Test {
	public class SlidingTtlCacheMgmt : CacheManagement<MyData> {
		public SlidingTtlCacheMgmt(ILogger<SlidingTtlCacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new SlidingTtl(TimeSpan.FromSeconds(1));
	}
}
