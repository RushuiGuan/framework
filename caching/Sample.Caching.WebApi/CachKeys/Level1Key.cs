using Albatross.Caching;

namespace Sample.Caching.WebApi.CacheKeys {
	public class Level1Key : CacheKey {
		public Level1Key(string? value) : base("k1", value, true) { }
	}
}
