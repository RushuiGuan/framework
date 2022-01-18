﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching.Test {
	public class YourCacheMgmt : CacheManagement<string> {
		public YourCacheMgmt(ILogger<YourCacheMgmt> logger, IPolicyRegistry<string> registry, IAsyncCacheProvider cacheProvider, IMemoryCacheExtended cache) : base(logger, registry, cacheProvider, cache) {
		}

		public override string Name => nameof(YourCacheMgmt);

		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(5));
	}
}