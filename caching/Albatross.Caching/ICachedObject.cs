namespace Albatross.Caching {
	public interface ICachedObject {
		void Invalidate(CacheEvictionService cacheEvictionService);
	}
}
