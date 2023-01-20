using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching.Test {
	public record class MyData { 
		public MyData(string value) {
			this.Value = value;
		}

		public string Value { get; set; }
	}
	public class LongTermCacheMgmt1 : CacheManagement<MyData> {
		public LongTermCacheMgmt1(ILogger<LongTermCacheMgmt1> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(1));
	}
	public class LongTermCacheMgmt2 : CacheManagement<MyData> {
		public LongTermCacheMgmt2(ILogger<LongTermCacheMgmt1> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(1));
	}
	public class LongTermCacheMgmt3 : CacheManagement<object> {
		public LongTermCacheMgmt3(ILogger<LongTermCacheMgmt1> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(1));
	}
}
