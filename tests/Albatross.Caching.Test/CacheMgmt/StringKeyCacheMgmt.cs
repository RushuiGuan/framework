using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Registry;
using System;

namespace Albatross.Caching.Test.CacheMgmt {
	public record class MyData {
		public MyData(string value) {
			Value = value;
		}
		public MyData() {
			Value = Guid.NewGuid().ToString();
		}

		public string Value { get; set; }
	}
	public class StringKeyCacheMgmt : CacheManagement<MyData, string> {
		public StringKeyCacheMgmt(ILogger<StringKeyCacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(1));
	}

	public class StringKey2CacheMgmt : CacheManagement<MyData, string> {
		public StringKey2CacheMgmt(ILogger<StringKey2CacheMgmt> logger, IPolicyRegistry<string> registry, ICacheProviderAdapter cacheProvider, ICacheKeyManagement keyMgmt) : base(logger, registry, cacheProvider, keyMgmt) {
		}
		public override ITtlStrategy TtlStrategy => new RelativeTtl(TimeSpan.FromDays(1));
	}
}
