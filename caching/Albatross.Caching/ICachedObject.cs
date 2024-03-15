using System.Collections.Generic;

namespace Albatross.Caching {
	public enum ObjectState {
		Added = 0, Modified = 1, Deleted = 2,
	}
	public interface ICachedObject<T> {
		IEnumerable<ICacheKey> CreateCacheKeys(ObjectState state, IEnumerable<T> changes);
	}
}
