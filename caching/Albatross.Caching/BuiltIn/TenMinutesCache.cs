﻿using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching.BuiltIn {
	public class TenMinutesCache<CacheFormat, KeyFormat> : Cache<CacheFormat, KeyFormat> where KeyFormat : ICacheKey {
		public TenMinutesCache(ILogger<TenMinutesCache<CacheFormat, KeyFormat>> logger,
				IPolicyRegistry<string> registry,
				ICacheProviderAdapter cacheProviderAdapter,
				ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProviderAdapter, keyMgmt) {
		}

		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(10));
	}
}