using Albatross.Caching;

namespace Sample.Caching.WebApi.CacheKeys {
	public class Tier1Key : CacheKey { 
		public Tier1Key(int t1Value) : base("t1", t1Value.ToString()) { } 
	}
}


// 1:2:3:
// 1:4:5:
// 2:1:1