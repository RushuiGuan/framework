using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Albatross.Collections {
	public static class Extensions {
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items) {
			foreach (var item in items) {
				collection.Add(item);
			}
		}

		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
			if (items != null) {
				foreach (var item in items) {
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

		public static bool TryGetAndRemove<K, T>(this IDictionary<K, T> dict, K key, [NotNullWhen(true)] out T? value) where T : notnull {
			if (dict.TryGetValue(key, out value)) {
				dict.Remove(key);
				return true;
			} else {
				return false;
			}
		}


		public static T GetOrAdd<T>(this ICollection<T> list, Func<T, bool> predicate, Func<T> func) {
			var item = list.FirstOrDefault(predicate);
			if (item == null) {
				item = func();
				list.Add(item);
			}
			return item;
		}

		public static bool TryGetAndRemove<T>(this ICollection<T> list, Func<T, bool> predicate,[NotNullWhen(true)] out T? item) {
			item = list.FirstOrDefault(predicate);
			if (item != null) {
				list.Remove(item);
				return true;
			} else {
				return false;
			}
		}
	}
}
