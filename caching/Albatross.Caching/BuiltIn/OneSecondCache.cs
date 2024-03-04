using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching.BuiltIn {
	public class OneSecondCache<CacheFormat, KeyFormat> : Cache<CacheFormat, KeyFormat> where KeyFormat : ICacheKey {
		public OneSecondCache(ILogger<OneSecondCache<CacheFormat, KeyFormat>> logger,
				IPolicyRegistry<string> registry,
				ICacheProviderAdapter cacheProviderAdapter,
				ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProviderAdapter, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromSeconds(1));
	}
}
