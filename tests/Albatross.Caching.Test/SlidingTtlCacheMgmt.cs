using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Albatross.Caching.Test {
	public class SlidingTtlCacheMgmt : CacheManagement<object> {
		public SlidingTtlCacheMgmt(ILogger<SlidingTtlCacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override string Name => nameof(SlidingTtlCacheMgmt);
		public override ITtlStrategy TtlStrategy => new SlidingTtl(TimeSpan.FromSeconds(1));
	}
}
