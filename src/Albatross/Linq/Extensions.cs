using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Linq {
	public static class Extensions {
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
			if(items != null) {
				foreach(var item in items) {
					action(item);
				}
			}
		}
	}
}
