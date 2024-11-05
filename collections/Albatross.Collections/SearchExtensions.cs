using System;
using System.Collections.Generic;

namespace Albatross.Collections {
	public static class SearchExtensions {
		public static Record? BinarySearchFirstValueGreaterOrEqual<Record, Key>(this IList<Record> list, Key key, Func<Record, Key> getKey)
			where Record : struct where Key : IComparable<Key> {
			int left = 0;
			int right = list.Count - 1;
			Record? result = default;

			if (key.CompareTo(getKey(list[0])) <= 0) {
				return list[0];
			} else if (key.CompareTo(getKey(list[list.Count - 1])) > 0) {
				return default;
			}
			while (left <= right) {
				int mid = left + (right - left) / 2;
				Record item = list[mid];
				Key value = getKey(item);

				if (value.CompareTo(key) >= 0) {
					result = item;
					right = mid - 1;
				} else {
					left = mid + 1;
				}
			}
			return result;
		}
		public static Record? BinarySearchFirstValueLessOrEqual<Record, Key>(this IList<Record> list, Key key, Func<Record, Key> getKey)
			where Record : struct
			where Key : IComparable<Key> {
			int left = 0;
			int right = list.Count - 1;
			Record? result = default;

			if (key.CompareTo(getKey(list[0])) < 0) {
				return default;
			} else if (key.CompareTo(getKey(list[list.Count - 1])) >= 0) {
				return list[list.Count - 1];
			}
			while (left <= right) {
				int mid = left + (right - left) / 2;
				Record item = list[mid];
				Key value = getKey(item);

				if (value.CompareTo(key) <= 0) {
					result = item;
					left = mid + 1;
				} else {
					right = mid - 1;
				}
			}
			return result;
		}

		public static Record? BinarySearchFirstGreaterOrEqual<Record, Key>(this IList<Record> list, Key key, Func<Record, Key> getKey)
			where Record : class where Key : IComparable<Key> {
			int left = 0;
			int right = list.Count - 1;
			Record? result = default;

			if (key.CompareTo(getKey(list[0])) <= 0) {
				return list[0];
			} else if (key.CompareTo(getKey(list[list.Count - 1])) > 0) {
				return default;
			}
			while (left <= right) {
				int mid = left + (right - left) / 2;
				Record item = list[mid];
				Key value = getKey(item);

				if (value.CompareTo(key) >= 0) {
					result = item;
					right = mid - 1;
				} else {
					left = mid + 1;
				}
			}
			return result;
		}
		public static Record? BinarySearchFirstLessOrEqual<Record, Key>(this IList<Record> list, Key key, Func<Record, Key> getKey)
			where Record : class
			where Key : IComparable<Key> {
			int left = 0;
			int right = list.Count - 1;
			Record? result = default;

			if (key.CompareTo(getKey(list[0])) < 0) {
				return default;
			} else if (key.CompareTo(getKey(list[list.Count - 1])) >= 0) {
				return list[list.Count - 1];
			}
			while (left <= right) {
				int mid = left + (right - left) / 2;
				Record item = list[mid];
				Key value = getKey(item);

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