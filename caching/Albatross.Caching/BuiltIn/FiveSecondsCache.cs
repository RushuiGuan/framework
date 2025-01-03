﻿using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching.BuiltIn {
	public class FiveSecondsCache<CacheFormat, KeyFormat> : Cache<CacheFormat, KeyFormat> where KeyFormat : ICacheKey {
		public FiveSecondsCache(ILogger<FiveSecondsCache<CacheFormat, KeyFormat>> logger,
				IPolicyRegistry<string> registry,
				ICacheProviderAdapter cacheProviderAdapter) : base(logger, registry, cacheProviderAdapter) {
		}

		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromSeconds(5));
	}
}