using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using StackExchange.Redis;
using System;

namespace Albatross.Caching.TestHost {
	public class MyCacheMgmt : CacheManagement<string> {
		public MyCacheMgmt(ILogger<MyCacheMgmt> logger, IPolicyRegistry<string> registry, IAsyncCacheProviderConverter cacheProvider, IRedisKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override string Name => nameof(MyCacheMgmt);
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(5));
	}
	public class MyCacheMgmt1 : CacheManagement<string> {
		public MyCacheMgmt1(ILogger<MyCacheMgmt1> logger, IPolicyRegistry<string> registry, IAsyncCacheProviderConverter cacheProvider, IRedisKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(5));
	}
}
