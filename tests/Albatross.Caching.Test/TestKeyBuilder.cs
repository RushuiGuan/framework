using Xunit;

namespace Albatross.Caching.Test {
	public class TestKeyBuilder {
		[Theory]
		[InlineData("a", "b", "a:b:")]
		[InlineData("A", "B", "a:b:")]
		[InlineData("A:", "B", "a:b:")]
		[InlineData("A", "B:", "a:b:")]
		[InlineData("A:", "B:", "a:b:")]
		public void CreateCompositeKey1(string key1, string key2, string expected) {
			var result = new KeyBuilder().Add(new string[] { key1, key2 }).Build(false);
			Assert.Equal(expected, result);
		}



		[Theory]
		[InlineData("a", "b", "a:b:")]
		[InlineData("a", "", "a:")]
		[InlineData("A:", "B", "a:b:")]
		[InlineData("A", "B:", "a:b:")]
		[InlineData("A:", "B:", "a:b:")]
		public void CreateCompositeKey2(string key1, string key2, string expected) {
			var result = new KeyBuilder().Add(new object[] { key1 }).Add(new object[] { key2 }).Build(false);
			Assert.Equal(expected, result);
		}
	}
}
