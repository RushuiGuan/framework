using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Linq {
	public static class Extensions {
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
			if(items != null) {
				foreach(var item in items) {
					action(item);
				}
			}
		}

		public static T GetOrAdd<K, T>(this IDictionary<K, T> dict, K key, Func<T> func) {
			if(!dict.TryGetValue(key, out T value)) {
				value = func();
				dict.Add(key, value);
			}
			return value;
		}
	}
}
