using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
	public class TestLiteralExpressionBuilder {
		[Theory]
		[InlineData("a", "\"a\"")]
		[InlineData("", "\"\"")]
		public void StringLiteral(string type, string expected) {
			var builder = new LiteralNode(type);
			var result = builder.ToString();
			Assert.Equal(expected, result);
		}
	}
}