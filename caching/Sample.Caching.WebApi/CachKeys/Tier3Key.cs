using Albatross.Caching;
using Sample.Caching.WebApi.CacheKeys;

namespace Sample.Caching.WebApi {
	public class Tier3Key : CacheKey {
		public Tier3Key(int t1Value, int t2Value, int t3Value) 
			: base(new Tier2Key(t1Value, t2Value), "t3", t3Value.ToString(), false) { }
	}
}

// 1:2:3:
// 1:4:5:
// 2:1:1