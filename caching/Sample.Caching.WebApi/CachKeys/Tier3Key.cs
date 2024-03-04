using Albatross.Caching;

namespace Sample.Caching.WebApi {
	public class Tier1Key : CacheKey { 
		public Tier1Key(int t1Value) : base("t1", t1Value.ToString()) { } 
	}
	public class Tier2Key : CacheKey{
		public Tier2Key(int t1Value, int t2Value) : base(new Tier1Key(t1Value), "t2", t2Value.ToString()) { }
	}
	public class Tier3Key : CacheKey {
		public Tier3Key(int t1Value, int t2Value, int t3Value) : base(new Tier2Key(t1Value, t2Value), "t3", t3Value.ToString()) { }
	}
}


// 1:2:3:
// 1:4:5:
// 2:1:1