using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	public static class StackExtensions {
		public static IEnumerable<T> PopUntil<T>(this Stack<T> stack, Func<T, bool> predicate, bool discard) {
			while (stack.Any()) {
				var item = stack.Pop();
				if (predicate(item)) {
					if (!discard) {
						yield return item;
					}
					break;
				} else {
					yield return item;
				}
			}
		}
	}
}