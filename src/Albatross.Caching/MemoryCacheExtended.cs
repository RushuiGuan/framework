using Albatross.Linq;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Albatross.Caching {
	public interface IMemoryCacheExtended {
		IEnumerable<object> Keys { get; }
		void Envict(IEnumerable<object> keys);
		void Reset();
	}
	/// <summary>
	/// an extension to the MemoryCache class that provides the ability to retrieve cache keys and reset cache
	/// </summary>
	public class MemoryCacheExtended : IMemoryCacheExtended {
		private readonly MemoryCache cache;

		public MemoryCacheExtended(IMemoryCache cache) {
			if (!(cache is MemoryCache)) {
				throw new InvalidOperationException("MemoryCacheReset can only be used with MemoryCache");
			}
			this.cache = (MemoryCache)cache;
		}

		public IEnumerable<object> Keys {
			get {
				FieldInfo? field = typeof(MemoryCache).GetField("_entries", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				IDictionary? dictionary = (IDictionary?)(field?.GetValue(cache));
				return dictionary?.Keys.Cast<object>() ?? new object[0];
			}
		}

		public void Envict(IEnumerable<object> keys) => keys.ForEach(this.cache.Remove);
		public void Reset() => Keys.ForEach(this.cache.Remove);
	}
}
