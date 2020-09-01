using Albatross.Caching;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using System;

namespace Albatross.Hosting.Test {
	public class SymbolCacheManagement : CacheManagement<int[]> {
		public const string CacheName = "sy";
		public SymbolCacheManagement(ILogger<SymbolCacheManagement> logger) : base(logger) {
		}

		public override string Name => CacheName;

		public override ITtlStrategy TtlStrategy => TimeSpan.FromMinutes(10).GetTtlStrategy();
	}
}