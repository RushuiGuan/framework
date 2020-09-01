using Albatross.Caching;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using System;

namespace Albatross.Hosting.Test {
	public class IssuerCachedMgmt : CacheManagement<string[]> {
		public const string CacheName = "issuer";

		public IssuerCachedMgmt(ILogger<IssuerCachedMgmt> logger) : base(logger) {
		}

		public override string Name => CacheName;
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromMinutes(5));
	}
}