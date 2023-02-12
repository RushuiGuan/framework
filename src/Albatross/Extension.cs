using System;
using System.Collections;


namespace Albatross {
	/// <summary>
	/// Utilities in the root Albatross namespace typically are system utilities that is so low level, it doesn't have a
	/// category.  Microsoft use namespace "System" for them.  We could consider Albatross.System but Albatross seems fitting.
	/// </summary>
	public static class Extension {
		public static HashCode FromCollection(this HashCode hashCode, IEnumerable items) {
			foreach(var item in items) {
				hashCode.Add(item);
			}
			return hashCode;
		}
		public static HashCode From<T>(this HashCode hashCode, T item) {
			hashCode.Add(item);
			return hashCode;
		}
	}
}
