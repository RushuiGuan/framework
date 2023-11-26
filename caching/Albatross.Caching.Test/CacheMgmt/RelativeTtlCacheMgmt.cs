using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching.Test.CacheMgmt {
	public class RelativeTtlCacheMgmt : CacheManagement<object, string> {
		public RelativeTtlCacheMgmt(ILogger<RelativeTtlCacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromSeconds(1));
	}
}
