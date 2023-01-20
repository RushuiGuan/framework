using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Linq {
	public static class Extensions {
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
		public static void Merge<Src, Dst, TKey>(this IEnumerable<Dst> dst, IEnumerable<Src> src,
			Func<Src, TKey> srcKeySelector, Func<Dst, TKey> dstKeySelector,
			Action<Src, Dst>? matched, Action<Src>? notMatchedByDst, Action<Dst>? notMatchedBySrc) where TKey : notnull {
			var dstArray = dst.ToArray();
			if (src == null) { src = new Src[0]; }
			Dictionary<TKey, Src> srcDict = new Dictionary<TKey, Src>();
			List<Src> newItems = new List<Src>();

			foreach (var item in src) {
				TKey key = srcKeySelector(item);
				if (object.Equals(key, default(TKey))) {
					newItems.Add(item);
				} else {
					srcDict.Add(key, item);
				}
			}
			foreach (var item in dstArray) {
				TKey key = dstKeySelector(item);
				if (srcDict.TryGetValue(key, out Src srcItem)) {
					matched?.Invoke(srcItem, item);
					srcDict.Remove(key);
				} else {
					notMatchedBySrc?.Invoke(item);
				}
			}
			foreach (var item in srcDict.Values) {
				notMatchedByDst?.Invoke(item);
			}
			foreach (var item in newItems) {
				notMatchedByDst?.Invoke(item);
			}
		}

		public static async Task MergeAsync<Src, Dst, TKey>(this IEnumerable<Dst> dst, IEnumerable<Src> src,
			Func<Src, TKey> srcKeySelector, Func<Dst, TKey> dstKeySelector,
			Func<Src, Dst, Task>? matched, 
			Func<Src, Task>? notMatchedByDst, 
			Func<Dst, Task>? notMatchedBySrc) where TKey : notnull {

			var dstArray = dst.ToArray();
			if (src == null) { src = new Src[0]; }
			Dictionary<TKey, Src> srcDict = new Dictionary<TKey, Src>();
			List<Src> newItems = new List<Src>();

			foreach (var item in src) {
				TKey key = srcKeySelector(item);
				if (object.Equals(key, default(TKey))) {
					newItems.Add(item);
				} else {
					srcDict.Add(key, item);
				}
			}
			foreach (var item in dstArray) {
				TKey key = dstKeySelector(item);
				if (srcDict.TryGetValue(key, out Src? srcItem)) {
					if (matched != null) {
						await matched(srcItem, item);
					}
					srcDict.Remove(key);
				} else {
					if (notMatchedBySrc != null) {
						await notMatchedBySrc(item);
					}
				}
			}
			if (notMatchedByDst != null) {
				foreach (var item in srcDict.Values) {
					await notMatchedByDst(item);
				}
				foreach (var item in newItems) {
					await notMatchedByDst(item);
				}
			}
		}
	}
}
