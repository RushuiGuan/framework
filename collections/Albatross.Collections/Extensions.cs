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
			if (!dict.TryGetValue(key, out T value)) {
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

		[Obsolete($"Replaced with {nameof(TryGetOneAndRemove)}")]
		public static bool TryGetAndRemove<T>(this ICollection<T> list, Func<T, bool> predicate, [NotNullWhen(true)] out T? item) {
			item = list.FirstOrDefault(predicate);
			if (item != null) {
				list.Remove(item);
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Return true if an item that matches the predicate is found.  If found, the item is removed from the list and returned as an out parameter.
		/// </summary>
		public static bool TryGetOneAndRemove<T>(this IList<T> list, Func<T, bool> predicate, [NotNullWhen(true)] out T? item) where T : notnull {
			for (var i = 0; i < list.Count; i++) {
				if (predicate(list[i])) {
					item = list[i];
					list.RemoveAt(i);
					return true;
				}
			}
			item = default;
			return false;
		}


		/// <summary>
		/// Remove items from the existing collection that have the same key as the new items, and then add the new items to the existing collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="K"></typeparam>
		/// <param name="existingItems"></param>
		/// <param name="newItems"></param>
		/// <param name="getKey"></param>
		[Obsolete("An odd method that is hard to understand.  Use AddRange and RemoveAny instead.")]
		public static void Replace<T, K>(this IList<T> existingItems, IEnumerable<T> newItems, Func<T, K> getKey) {
			if (newItems.Any()) {
				var keys = newItems.Select(getKey).ToHashSet();
				for (int i = existingItems.Count - 1; i >= 0; i--) {
					var item = existingItems.ElementAt(i);
					if (keys.Contains(getKey(item))) {
						existingItems.RemoveAt(i);
					}
				}
				existingItems.AddRange(newItems);
			}
		}

		public const int ListItemRemovalAlgoCutoff = 100;
		/// <summary>
		/// Remove items from a list based on the predicate.  Returns the removed item in a list.  If the list count is less or equal to the cut off,
		/// <see cref="RemoveAny_FromRear{T}(IList{T}, Predicate{T})"/> method is used.  Otherwise <see cref="RemoveAny_WithNewList{T}(IList{T}, Predicate{T})"/>
		/// method is used.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="predicate"></param>
		/// <param name="algoCutoff"></param>
		/// <returns></returns>
		public static IList<T> RemoveAny<T>(this IList<T> list, Predicate<T> predicate, int algoCutoff = ListItemRemovalAlgoCutoff) {
			if (list.Count > algoCutoff) {
				return list.RemoveAny_WithNewList(predicate);
			} else {
				return list.RemoveAny_FromRear(predicate);
			}
		}

		/// <summary>
		/// Remove any items from the list that match the predicate.  Use the List.RemoveAt method and remove from the back.  Performance is O(n).
		/// If the list size is larger than 1000 elements, use <see cref="RemoveAny_Linear{T}(IList{T}, Func{T, bool})"/>.  
		/// </summary>
		public static IList<T> RemoveAny_FromRear<T>(this IList<T> list, Predicate<T> predicate) {
			var removed = new List<T>();
			for (int i = list.Count - 1; i >= 0; i--) {
				if (predicate(list[i])) {
					removed.Add(list[i]);
					list.RemoveAt(i);
				}
			}
			return removed;
		}

		/// <summary>
		/// Remove all items from the list that match the predicate.  Copy the items that do not match to a new list.  Performance is n.
		/// Good for large list, slight performance penalty for list with 100 elements (100 - 200 ticks).  See the benchmark measure-listitem-removal for more details.
		/// </summary>
		public static IList<T> RemoveAny_WithNewList<T>(this IList<T> list, Predicate<T> predicate) {
			var removed = new List<T>();
			var newList = new List<T>();
			foreach (var item in list) {
				if (!predicate(item)) {
					newList.Add(item);
				} else {
					removed.Add(item);
				}
			}
			list.Clear();
			list.AddRange(newList);
			return removed;
		}

		public static ICollection<T> AddIfNotNull<T>(this ICollection<T> collection, params T?[] items) {
			foreach (var item in items) {
				if (item != null) {
					collection.Add(item);
				}
			}
			return collection;
		}
		public static ICollection<T> UnionAll<T>(this ICollection<T> collection, params IEnumerable<T>[] items) {
			foreach (var item in items) {
				collection.AddRange(item);
			}
			return collection;
		}
		public static T? Where<K, T>(this IDictionary<K, T> dict, K key, Func<T, bool>? predicate = null) where T : class {
			if (dict.TryGetValue(key, out var value)) {
				if (predicate == null || predicate(value)) {
					return value;
				} else {
					return null;
				}
			} else {
				return null;
			}
		}
		public static T? WhereValue<K, T>(this IDictionary<K, T> dict, K key, Func<T, bool>? predicate = null) where T : struct {
			if (dict.TryGetValue(key, out var value)) {
				if (predicate == null || predicate(value)) {
					return value;
				} else {
					return null;
				}
			} else {
				return null;
			}
		}
	}
}