namespace Albatross.Caching {
	public interface ICachedObject {
		bool InvalidateOnAnyChange { get; }
		void Invalidate(CacheEvictionService service);
	}
}
