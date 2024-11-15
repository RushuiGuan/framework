using Albatross.CodeGen.WebClient.Settings;
using Xunit;

namespace Albatross.CodeGen.UnitTest {
	public class SymbolFilterTest {
		[Theory]
		[InlineData("ABC", null, null, true)]
		[InlineData("ABC", "ABC", null, true)]
		[InlineData("ABC", "ABC", "ABC", true)]
		[InlineData("ABC", null, "ABC", false)]
		public void TestSinglePatternFilter(string text, string? include, string? exclude, bool shouldKeep) {
			var pattern = new SymbolFilterPatterns() { Include = include, Exclude = exclude };
			var filter = new SymbolFilter(pattern);
			Assert.Equal(shouldKeep, filter.ShouldKeep(text));
		}

		[Theory]
		[InlineData("ABC", null, null, null, null, true)]
		[InlineData("ABC", null, "ABC", null, null, false)]
		[InlineData("ABC", "ABC", "ABC", null, null, true)]
		[InlineData("ABC", null, "ABC", "ABC", null, true)]
		public void TestMultiplePatternFilter(string text, string? include1, string? exclude1, string? include2, string? exclude2, bool expected) {
			var pattern = new SymbolFilterPatterns() { Include = include1, Exclude = exclude1 };
			var filter1 = new SymbolFilter(pattern);
			pattern = new SymbolFilterPatterns() { Include = include2, Exclude = exclude2 };
			var filter2 = new SymbolFilter(pattern);
			var filters = new SymbolFilter[] { filter1, filter2 };

			var result = SymbolFilter.ShouldKeep(filters, text);
			Assert.Equal(expected, result);
		}
	}
}
