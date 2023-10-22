using Xunit;

namespace Albatross.Excel.Test {
	public class TestCellValueUtility {

		[Theory]
		[InlineData("true", true, true)]
		[InlineData("false", true, false)]
		[InlineData("True", true, true)]
		[InlineData("False", true, false)]
		[InlineData("TRUE", true, true)]
		[InlineData("FALSE", true, false)]
		[InlineData("", false, false)]
		public void TestParsingBool(string text, bool expectedParsed, bool expectedResult) {
			var parsed = bool.TryParse(text, out var result);
			Assert.Equal(expectedParsed, parsed);
			Assert.Equal(expectedResult, result); 
		}
	}
}