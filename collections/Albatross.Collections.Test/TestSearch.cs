using Albatross.Testing;
using System.Linq;
using Xunit;

namespace Albatross.Collections.Test {
	public class TestSearch {

		[Theory]
		[InlineData("1-10", 5, 5)]
		[InlineData("1-10", 0, 1)]
		[InlineData("1-10", 11, null)]
		[InlineData("1,3,5,7,9,11", 4, 5)]
		[InlineData("1,10", 4, 10)]
		public void TestGreaterOrEqualSearch_Value(string text, int target, int? expected) {
			var array = text.IntArray();
			var value = array.BinarySearchFirstValueGreaterOrEqual<int, int>(target, x => x);
			Assert.Equal(expected, value);
		}

		[Theory]
		[InlineData("1-10", 5, 5)]
		[InlineData("1-10", 0, null)]
		[InlineData("1-10", 11, 10)]
		[InlineData("1,3,5,7,9,11", 4, 3)]
		[InlineData("1,10", 4, 1)]
		public void TestLessOrEqualSearch_Value(string text, int target, int? expected) {
			var array = text.IntArray();
			var value = array.BinarySearchFirstValueLessOrEqual<int, int>(target, x => x);
			Assert.Equal(expected, value);
		}

		public class TestClass {
			public TestClass(int value) {
				this.Value = value;
			}
			public int Value { get; set; }
		}

		[Theory]
		[InlineData("1-10", 5, 5)]
		[InlineData("1-10", 0, 1)]
		[InlineData("1-10", 11, null)]
		[InlineData("1,3,5,7,9,11", 4, 5)]
		[InlineData("1,10", 4, 10)]
		public void TestGreaterOrEqualSearch(string text, int target, int? expected) {
			var array = text.IntArray().Select(x => new TestClass(x)).ToArray();
			var value = array.BinarySearchFirstGreaterOrEqual(target, x => x.Value);
			Assert.Equal(expected, value?.Value);
		}

		[Theory]
		[InlineData("1-10", 5, 5)]
		[InlineData("1-10", 0, null)]
		[InlineData("1-10", 11, 10)]
		[InlineData("1,3,5,7,9,11", 4, 3)]
		[InlineData("1,10", 4, 1)]
		public void TestLessOrEqualSearch(string text, int target, int? expected) {
			var array = text.IntArray().Select(x => new TestClass(x)).ToArray();
			var value = array.BinarySearchFirstLessOrEqual(target, x => x.Value);
			Assert.Equal(expected, value?.Value);
		}
	}
}