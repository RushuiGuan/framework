using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching.Test.CacheMgmt {
	public class MyKey {
		public MyKey(string value) {
			Value = value;
		}
		public string Value { get; set; }
		public override string ToString() => Value;
	}

	public class CustomKeyCacheMgmt : CacheManagement<MyData, MyKey> {
		public CustomKeyCacheMgmt(ILogger logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProviderAdapter, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProviderAdapter, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(1));
	}
}
