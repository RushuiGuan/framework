using Xunit;

namespace Albatross.Caching.Test {
	public class TestCompositeKeyBuilder {
		[Theory]
		[InlineData("a", "b", "a:b:")]
		[InlineData("A", "B", "a:b:")]
		[InlineData("A:", "B", "a:b:")]
		[InlineData("A", "B:", "a:b:")]
		[InlineData("A:", "B:", "a:b:")]
		public void CreateCompositeKey1(string key1, string key2, string expected) {
			var result = new CompositeKeyBuilder(key1, key2).Build(false);
			Assert.Equal(expected, result);
		}



		[Theory]
		[InlineData("a", "b", "a:b:")]
		[InlineData("a", "", "a:")]
		[InlineData("A:", "B", "a:b:")]
		[InlineData("A", "B:", "a:b:")]
		[InlineData("A:", "B:", "a:b:")]
		public void CreateCompositeKey2(string key1, string key2, string expected) {
			var result = new CompositeKeyBuilder(key1).Add(key2).Build(false);
			Assert.Equal(expected, result);
		}
	}
}
