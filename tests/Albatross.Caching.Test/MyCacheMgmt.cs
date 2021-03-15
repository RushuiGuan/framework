using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching.Test {
	public class MyCacheMgmt : CacheManagement<string> {
		public MyCacheMgmt(ILogger<MyCacheMgmt> logger, IPolicyRegistry<string> registry, IAsyncCacheProvider cacheProvider, IMemoryCacheExtended cache) : base(logger, registry, cacheProvider, cache) {
		}

		public override string Name => nameof(MyCacheMgmt);

		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(5));
	}
}
