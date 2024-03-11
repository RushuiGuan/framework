using System.Collections.Generic;

namespace Albatross.Caching {
	public interface ICachedObject {
		ICacheKey CreateCacheKey(int state, object? originalValues);
	}
}
