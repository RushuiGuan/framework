using Albatross.Caching;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Authentication.Server {
	public class ActiveDirectoryUserProfileCacheMgmt : Albatross.Caching.CacheManagement<User> {
		public ActiveDirectoryUserProfileCacheMgmt(ILogger logger, IPolicyRegistry<string> registry, IAsyncCacheProvider cacheProvider, IMemoryCacheExtended cache) : base(logger, registry, cacheProvider, cache) {
		}

		public const string CacheKey = nameof(ActiveDirectoryUserProfileCacheMgmt);
		public override string Name => CacheKey;

		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.MaxValue);
	}
}
