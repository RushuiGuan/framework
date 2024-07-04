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

		/// <summary>
		/// Remove items from the existing collection that have the same key as the new items, and then add the new items to the existing collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="K"></typeparam>
		/// <param name="existingItems"></param>
		/// <param name="newItems"></param>
		/// <param name="getKey"></param>
		public static void Replace<T, K>(this IList<T> existingItems, IEnumerable<T> newItems, Func<T, K> getKey) {
			if (newItems.Any()) {
				var keys = newItems.Select(getKey).ToHashSet();
				for(int i = existingItems.Count - 1; i >= 0; i--) {
					var item = existingItems.ElementAt(i);
					if (keys.Contains(getKey(item))) {
						existingItems.RemoveAt(i);
					}
				}
				existingItems.AddRange(newItems);
			}
		}

		public static ICollection<T> AddIfNotNull<T>(this ICollection<T> collection, T? item) {
			if (item != null) {
				collection.Add(item);
			}
			return collection;
		}
		public static ICollection<T> UnionAll<T>(this ICollection<T> collection, params IEnumerable<T>[] items) {
			foreach (var item in items) {
				collection.AddRange(item);
			}
			return collection;
		}
	}
}
