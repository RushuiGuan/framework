using Polly;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Albatross.Caching {
	public static class My {
		public static Context Context(params object[] items) {
			return new Context(new CompositeKeyBuilder(items).Build(false));
		}

		public static void FindAndRemove(this ICacheKeyManagement keyMgmt, string pattern) {
			var keys = keyMgmt.FindKeys(pattern);
			keyMgmt.Remove(keys);
		}

		public static bool ClosureTest() {
			var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, };
			var funcs = new List<Func<int>>();
			foreach (var item in array) {
				funcs.Add(new Func<int>(() => item));
			}
			var result = funcs.Select(x => x()).ToArray();
			return array.SequenceEqual(result);
		}
	}
}