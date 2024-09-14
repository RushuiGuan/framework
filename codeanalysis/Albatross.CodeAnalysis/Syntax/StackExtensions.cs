using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	public static class CollectionExtensions {
		public static Stack<T> PopUntil<T>(this Stack<T> stack, Func<T, bool> predicate, out T last) {
			last = default!;
			var localStack = new Stack<T>();
			while (stack.Any()) {
				var item = stack.Pop();
				if (predicate(item)) {
					last = item;
					return localStack;
				} else {
					localStack.Push(item);
				}
			}
			throw new InvalidOperationException("None of stack item matched the predicate");
		}
	}
}