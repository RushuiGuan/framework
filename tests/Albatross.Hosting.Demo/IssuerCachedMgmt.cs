using Albatross.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Hosting.Test {
	public class IssuerCachedMgmt : CacheManagement<string[]> {
		public const string CacheName = "issuer";

		public IssuerCachedMgmt(ILogger logger, IPolicyRegistry<string> registry, IAsyncCacheProvider cacheProvider, IMemoryCache cache) : base(logger, registry, cacheProvider, cache) {
		}

		public override string Name => CacheName;
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(5));
	}
}