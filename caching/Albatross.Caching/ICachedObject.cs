using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Caching {
	public class CachedObject<T, K> : ICachedObject where T : ICacheManagement<K> {
		public Func<object> GetCacheKey => throw new NotImplementedException();

		public Type CacheMgmtType => throw new NotImplementedException();
	}

	public interface ICachedObject<Key> {
		Func<Key> GetCacheKey{get; }
		Type CacheMgmtType{get; }
	}
}
