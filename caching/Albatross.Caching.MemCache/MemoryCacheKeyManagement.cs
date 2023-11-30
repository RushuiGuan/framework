using Albatross.Text;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Albatross.Caching {
	/// <summary>
	/// an extension to the MemoryCache class that provides the ability to retrieve cache keys and reset cache
	/// </summary>
	public class MemoryCacheKeyManagement : ICacheKeyManagement {
		private readonly MemoryCache cache;
		private readonly IDictionary keys;

		public MemoryCacheKeyManagement(IMemoryCache cache) {
			if(!(cache is MemoryCache)) {
				throw new InvalidOperationException($"{nameof(MemoryCacheKeyManagement)} can only be used with MemoryCache");
			}
			this.cache = (MemoryCache)cache;
			this.keys = GetMemoryCacheCacheEntryDictionaryByReflection(this.cache);
		}

		private IDictionary GetMemoryCacheCacheEntryDictionaryByReflection(MemoryCache cache) {
			FieldInfo? field = typeof(MemoryCache).GetField("_coherentState", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			var obj = field.GetValue(cache);
			Type? type = obj?.GetType();
			FieldInfo? entryField = type?.GetField("_entries", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			IDictionary? dictionary = (IDictionary?)(entryField?.GetValue(obj));
			return dictionary ?? throw new InvalidOperationException("Unable to get the CacheEntryDictionary object from MemoryCache by reflection");
		}
		public Task Init() => Task.CompletedTask;

		public IEnumerable<object> Keys => this.keys.Keys.Cast<object>();

		public string[] FindKeys(string pattern) {
			return this.Keys.Select(args => Convert.ToString(args)).Where(args => args.Like(pattern)).ToArray();
		}
		public void Remove(string[] keys) {
			foreach (var item in keys) {
				this.cache.Remove(item);
			}
		}
	}
}