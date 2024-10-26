using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Albatross.EFCore.Test {
	public class TestQueryExecution {
		public static readonly int[] Array = new int[] {
			1, 2, 3, 4, 5,
		};


		[Fact]
		public void RunEnumeration() {
			IEnumerable<int> result = Array.Select(args => {
				Console.WriteLine($"original value: {args}");
				return args * 100;
			});

			foreach (var item in result) {
				Console.WriteLine($"loop result {item}");
			}
		}
	}
}