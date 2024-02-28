using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Caching {
	public interface ICachedObject<T, K> where T: ICacheManagement<K> {
		K GetCacheKey();
	}
}
