using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Repository.Core {
	public static class Extension {
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items) {
			foreach (var item in items) {
				collection.Add(item);
			}
		}

		public static Task<int> SaveChangesAsync<T>(this IRepository<T> repo, CancellationToken cancellationToken = default) {
			return repo.DbSession.SaveChangesAsync(cancellationToken);
		}

		public static void Merge<Src, Dst, TKey>(this IEnumerable<Dst> dst, IEnumerable<Src> src,
			Func<Src, TKey> srcKeySelector, Func<Dst, TKey> dstKeySelector, 
			Action<Src, Dst>? matched, Action<Src>? notMatchedByDst, Action<Dst>? notMatchedBySrc) where TKey:notnull{
			var dstArray = dst.ToArray();
			if(src == null) { src = new Src[0]; }
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
	}
}