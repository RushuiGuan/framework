using Albatross.Caching;

namespace Sample.Caching.WebApi.CacheKeys {
	public class Level2Key : CacheKey {
		public Level2Key(string? level1, string? level2) : base(new Level1Key(level1), "k2", level2) { }
	}
}
