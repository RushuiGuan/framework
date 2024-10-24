using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Collections {
	public static class SearchExtensions {
		public static T? BinarySearchFirstValueGreaterOrEqual<T, K>(this IList<T> list, K key, Func<T, K> selector) where T : struct where K : IComparable<K> {
			int left = 0;
			int right = list.Count - 1;
			T? result = default;

			while (left <= right) {
				int mid = left + (right - left) / 2;
				T item = list[mid];
				K value = selector(item);

				if (value.CompareTo(key) >= 0) {
					result = item;
					right = mid - 1;
				} else {
					left = mid + 1;
				}
			}
			return result;
		}
		public static T? BinarySearchFirstValueLessOrEqual<T, K>(this IList<T> list, K key, Func<T, K> selector)
			where T : struct
			where K : IComparable<K> {
			int left = 0;
			int right = list.Count - 1;
			T? result = default;

			while (left <= right) {
				int mid = left + (right - left) / 2;
				T item = list[mid];
				K value = selector(item);

				if (value.CompareTo(key) <= 0) {
					result = item;
					left = mid + 1;
				} else {
					right = mid - 1;
				}
			}

			return result;
		}

		public static T? BinarySearchFirstGreaterOrEqual<T, K>(this IList<T> list, K key, Func<T, K> selector)
			where T : class where K : IComparable<K> {
			int left = 0;
			int right = list.Count - 1;
			T? result = default;

			while (left <= right) {
				int mid = left + (right - left) / 2;
				T item = list[mid];
				K value = selector(item);

				if (value.CompareTo(key) >= 0) {
					result = item;
					right = mid - 1;
				} else {
					left = mid + 1;
				}
			}
			return result;
		}
		public static T? BinarySearchFirstLessOrEqual<T, K>(this IList<T> list, K key, Func<T, K> selector)
			where T : class
			where K : IComparable<K> {
			int left = 0;
			int right = list.Count - 1;
			T? result = default;

			while (left <= right) {
				int mid = left + (right - left) / 2;
				T item = list[mid];
				K value = selector(item);

				if (value.CompareTo(key) <= 0) {
					result = item;
					left = mid + 1;
				} else {
					right = mid - 1;
				}
			}

			return result;
		}
	}
}
