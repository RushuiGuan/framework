using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	public static class CollectionExtensions {
		public static List<T> PopUntil<T>(this Stack<T> stack, Func<T, bool> predicate, out T last) {
			last = default(T)!;
			var list = new List<T>();
			while (stack.Any()) {
				var item = stack.Pop();
				if (predicate(item)) {
					last = item;
					return list;
				} else {
					list.Add(item);
				}
			}
			throw new InvalidOperationException("None of stack item matched the predicate");
		}
	}
}