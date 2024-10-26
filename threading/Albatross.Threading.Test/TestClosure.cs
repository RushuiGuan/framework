using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.Thrading.Test {
	public class TestClosure {
		[Fact]
		public void CheckCreation() {
			var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, };
			var funcs = new List<Func<int>>();
			foreach (var item in array) {
				funcs.Add(new Func<int>(() => item));
			}

			var result = funcs.Select(x => x()).ToArray();

			Assert.True(array.SequenceEqual(result));
		}
	}
}