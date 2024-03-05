using Xunit;

namespace Albatross.Caching.Test {
	public class TestCacheKey {
		[Theory]
		[InlineData("prefix", "red", "prefix:red:", "prefix:red:*", "prefix:*")]
		[InlineData("prefix", null, "prefix:", "prefix:*", "prefix:*")]
		[InlineData("prefix", "", "prefix:", "prefix:*", "prefix:*")]
		[InlineData("", "xx", "df:xx:", "df:xx:*", "df:*")]
		public void TestCacheKeyCreation_NoParent(string keyPrefix, string? keyValue, 
			string expectedKey, 
			string expectedWildCardKey, 
			string expectedResetKey) {

			var key = new CacheKey(keyPrefix, keyValue);
			Assert.Equal(expectedKey, key.Key);
			Assert.Equal(expectedResetKey, key.ResetKey);
			Assert.Equal(expectedWildCardKey, key.WildCardKey);
		}

		[Theory]
		[InlineData("parent", "red", "prefix", "blue", "parent:red:prefix:blue:", "parent:red:prefix:blue:*", "parent:red:prefix:*")]
		[InlineData("parent", "", "prefix", "blue", "parent:prefix:blue:", "parent:prefix:blue:*", "parent:prefix:*")]
		[InlineData("parent", "red", "prefix", "", "parent:red:prefix:", "parent:red:prefix:*", "parent:red:prefix:*")]
		[InlineData("parent", "", "prefix", "", "parent:prefix:", "parent:prefix:*", "parent:prefix:*")]
		public void TestCacheKeyCreation_WithParent(string parentPrefix, string? parentValue, string keyPrefix, string? keyValue,
			string expectedKey,
			string expectedWildCardKey,
			string expectedResetKey) {

			var key = new CacheKey(new CacheKey(parentPrefix, parentValue), keyPrefix, keyValue);
			Assert.Equal(expectedKey, key.Key);
			Assert.Equal(expectedResetKey, key.ResetKey);
			Assert.Equal(expectedWildCardKey, key.WildCardKey);
		}
	}
}