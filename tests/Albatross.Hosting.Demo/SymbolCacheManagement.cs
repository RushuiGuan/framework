﻿using Albatross.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Collections.Generic;

namespace Albatross.Hosting.Test {
	public class SymbolCacheManagement : CacheManagement<IEnumerable<string>> {
		public const string CacheName = "sy";

		public SymbolCacheManagement(ILogger logger, IPolicyRegistry<string> registry, IAsyncCacheProvider cacheProvider, IMemoryCacheExtended cache) : base(logger, registry, cacheProvider, cache) {
		}

		public override string Name => CacheName;

		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(10));
	}
}