using System.Collections.Generic;

namespace Albatross.Caching {
	public enum ObjectState {
		Added = 0, Modified = 1, Deleted = 2,
	}
	public interface ICachedObject<Context, PropertyEntryType> {
		IEnumerable<ICacheKey> CreateCacheKeys(ObjectState state, Context context, IEnumerable<PropertyEntryType> changes);
	}
}
