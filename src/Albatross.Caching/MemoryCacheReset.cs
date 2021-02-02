using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Albatross.Caching {
	/// <summary>
	/// a special implementation to reset the memory cache.
	/// </summary>
	public class MemoryCacheReset : IMemoryCacheReset {
		private readonly MemoryCache cache;

		public MemoryCacheReset(IMemoryCache cache) {
			if(!(cache is MemoryCache)) {
				throw new InvalidOperationException("MemoryCacheReset can only be used with MemoryCache");
			}

			this.cache = (MemoryCache)cache;
		}

		public void Reset() {
			FieldInfo field = typeof(MemoryCache).GetField("_entries", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			IDictionary dictionary  = (IDictionary) field.GetValue(cache);
			var array = dictionary.Keys.OfType<object>().ToArray();
			foreach(var key in array) {
				cache.Remove(key);
			}
		}
	}
}
