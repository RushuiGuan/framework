using System.Collections.Generic;

namespace Albatross.Caching {
	public interface ICachedObject {
		IEnumerable<ICacheKey> CacheKeys { get; }
	}
}
