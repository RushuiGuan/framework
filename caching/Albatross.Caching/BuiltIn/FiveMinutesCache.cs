using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching.BuiltIn {
	public class FiveMinutesCache<CacheFormat, KeyFormat> : Cache<CacheFormat, KeyFormat> where KeyFormat : ICacheKey {
		public FiveMinutesCache(ILogger<FiveMinutesCache<CacheFormat, KeyFormat>> logger,
				IPolicyRegistry<string> registry,
				ICacheProviderAdapter cacheProviderAdapter) : base(logger, registry, cacheProviderAdapter) {
		}

		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(5));
	}
}