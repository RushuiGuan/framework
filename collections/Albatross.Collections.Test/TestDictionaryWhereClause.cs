using System.Collections.Generic;
using Xunit;

namespace Albatross.Collections.Test {
	public class TestDictionaryWhereClause {
		[Fact]
		public void TestNonNullableReferenceType() {
			var dict = new Dictionary<int, string> {
				{ 1, "one" },
				{ 2, "two" }
			};
			var result = dict.Where(1, x => x.StartsWith("o"));
			Assert.Equal("one", result);

			result = dict.Where(3, x => x.StartsWith("o"));
			Assert.Null(result);

			result = dict.Where(2, x => x.StartsWith("x"));
			Assert.Null(result);
		}


		[Fact]
		public void TestValueType() {
			var dict = new Dictionary<int, int> {
				{ 1, 4 },
				{ 2, 5 }
			};
			var result = dict.WhereValue(1, x => x % 2 == 0);
			Assert.Equal(4, result);

			result = dict.WhereValue(2, x => x % 2 == 0);
			Assert.Null(result);
		}
	}
}
