using Polly;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Albatross.Caching {
	public static class My {
		public static Context Context(params object[] items) {
			return new Context(new KeyBuilder().Add(items).Build(false));
		}

		public static void FindAndRemove(this ICacheKeyManagement keyMgmt, string pattern) {
			var keys = keyMgmt.FindKeys(pattern);
			keyMgmt.Remove(keys);
		}
	}
}