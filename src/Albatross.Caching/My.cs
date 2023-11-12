using Polly;

namespace Albatross.Caching {
	public static class My {
		public static Context Context(params object[] items) {
			return new Context(new CompositeKeyBuilder(items).Build(false));
		}

		public static void FindAndRemove(this ICacheKeyManagement keyMgmt, string pattern) {
			var keys = keyMgmt.FindKeys(pattern);
			keyMgmt.Remove(keys);
		}
	}
}