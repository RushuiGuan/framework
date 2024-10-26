using Albatross.Caching;

namespace Sample.Caching.WebApi.CacheKeys {
	public class Tier2Key : CacheKey {
		public Tier2Key(int t1Value, int t2Value)
			: base(new Tier1Key(t1Value), "t2", t2Value.ToString(), true) { }
	}
}


// 1:2:3:
// 1:4:5:
// 2:1:1