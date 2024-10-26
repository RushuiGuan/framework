using Xunit;

namespace Albatross.Text {
	public class TestCamelCaseVariableNameConversion {

		[Theory]
		[InlineData("rushui", "rushui")]
		[InlineData("rushui guan", "rushui guan")]
		[InlineData("", "")]
		[InlineData(null, null)]
		[InlineData("RushuiGuan", "rushuiGuan")]
		[InlineData("Rushui Guan", "rushui Guan")]
		[InlineData("Rushui", "rushui")]
		[InlineData("CUSIP", "cusip")]
		[InlineData("BBYellow", "bbYellow")]
		public void Run(string? input, string? expected) {
			var result = input?.CamelCase();
			Assert.Equal(expected, result);
		}
	}
}