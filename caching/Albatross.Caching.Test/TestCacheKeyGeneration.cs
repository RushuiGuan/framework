using Xunit;

namespace Albatross.Caching.Test {
	public class TestCacheKeyGeneration {
		[Theory]
		[InlineData("prefix", "red", "prefix:red:", "prefix:red:*", "prefix:*")]
		[InlineData("prefix", null, "prefix:", "prefix:*", "prefix:*")]
		[InlineData("prefix", "", "prefix:", "prefix:*", "prefix:*")]
		[InlineData("", "xx", "df:xx:", "df:xx:*", "df:*")]
		public void TestKeyGeneration_StringKeyCacheMgmt(string keyPrefix, string? keyValue, 
			string expectedKey, 
			string expectedWildCardKey, 
			string expectedResetKey) {

			var key = new CacheKey(keyPrefix, keyValue);
			Assert.Equal(expectedKey, key.Key);
			Assert.Equal(expectedResetKey, key.ResetKey);
			Assert.Equal(expectedWildCardKey, key.WildCardKey);
		}
	}
}