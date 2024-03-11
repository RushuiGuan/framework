namespace Albatross.Caching {
	public enum ObjectState {
		Added = 0, Modified = 1, Deleted = 2,
	}
	public interface ICachedObject {
		ICacheKey CreateCacheKey(ObjectState state, object? originalValues);
	}
}
