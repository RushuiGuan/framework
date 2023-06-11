using System;
using System.Collections;


namespace Albatross {
	/// <summary>
	/// Utilities in the root Albatross namespace typically are system utilities that is so low level, it doesn't have a
	/// category.  Microsoft use namespace "System" for them.  We could consider Albatross.System but Albatross seems fitting.
	/// </summary>
	public static class Extensions {
		/// <summary>
		/// Convinient method that add collection values to the hash code and return the original hash code object.  This allows the caller to chain multiple Add together.
		/// </summary>
		public static HashCode FromCollection(this HashCode hashCode, IEnumerable items) {
			foreach(var item in items) {
				hashCode.Add(item);
			}
			return hashCode;
		}
		/// <summary>
		/// Convinient method that add a single value to the hash code and return the original hash code object.  This allows the caller to chain multiple Add together.
		/// </summary>
		public static HashCode From<T>(this HashCode hashCode, T item) {
			hashCode.Add(item);
			return hashCode;
		}
	}
}
