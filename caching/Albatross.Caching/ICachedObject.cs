using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.Caching {
	public enum ObjectState {
		Added = 0, Modified = 1, Deleted = 2,
	}
	public interface ICachedObject<EntityEntryType, PropertyEntryType> {
		Task<IEnumerable<ICacheKey>> CreateCacheKeys(ObjectState state, EntityEntryType entityEntry, IEnumerable<PropertyEntryType> changes);
	}
}
