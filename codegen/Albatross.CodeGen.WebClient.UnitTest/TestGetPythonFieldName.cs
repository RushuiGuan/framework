using Xunit;

namespace Albatross.CodeGen.WebClient.UnitTest {
	public class TestGetPythonFieldName {
		[Theory]
		[InlineData("TestService", "test_service")]
		[InlineData("GDPService", "gdp_service")]
		[InlineData("AaBbCc", "aa_bb_cc")]
		[InlineData("A", "a")]
		[InlineData("a", "a")]
		[InlineData("aA", "a_a")]
		[InlineData("Aa", "aa")]
		[InlineData("ABC", "abc")]
		[InlineData("", "")]
		[InlineData("_", "_")]
		public void RunTest(string name, string expected) {
			var newName = CodeGen.Python.Conversions.Extensions.GetPythonFieldName(name);
			Assert.Equal(expected, newName);
		}	
	}
}
