using Albatross.Text;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Albatross.Caching {
	/// <summary>
	/// an extension to the MemoryCache class that provides the ability to retrieve cache keys and reset cache
	/// </summary>
	public class MemoryCacheKeyManagement : ICacheKeyManagement {
		private readonly MemoryCache cache;

		public MemoryCacheKeyManagement(IMemoryCache cache) {
			if(!(cache is MemoryCache)) {
				throw new InvalidOperationException($"{nameof(MemoryCacheKeyManagement)} can only be used with MemoryCache");
			}

			this.cache = (MemoryCache)cache;
		}

		public IEnumerable<object> Keys {
			get {
				FieldInfo? field = typeof(MemoryCache).GetField("_coherentState", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				var obj = field.GetValue(cache);
				Type? type = obj?.GetType();
				FieldInfo? entryField = type?.GetField("_entries", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				IDictionary? dictionary = (IDictionary?)(entryField?.GetValue(obj));
				return dictionary?.Keys.Cast<object>() ?? new object[0];
			}
		}

		public void FindAndRemoveKeys(string pattern) {
			var keys = FindKeys(pattern);
			Remove(keys);
		}
		public IEnumerable<string> FindKeys(string pattern) {
			return this.Keys.Select(args=>Convert.ToString(args))
				.Where(args => args.Like(pattern))
				.ToArray();
		}

		public void Remove(IEnumerable<string> keys) {
			foreach (string key in keys) { this.cache.Remove(key); }
		}
	}
}
