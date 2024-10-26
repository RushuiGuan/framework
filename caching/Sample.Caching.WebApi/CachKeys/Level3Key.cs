using Albatross.Caching;

namespace Sample.Caching.WebApi.CacheKeys {
	public class Level3Key : CacheKey {
		public Level3Key(string? level1, string? level2, string? level3)
			: base(new Level2Key(level1, level2), "k3", level3, false) { }
	}
}