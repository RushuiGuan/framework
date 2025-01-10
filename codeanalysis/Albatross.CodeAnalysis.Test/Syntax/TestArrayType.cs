using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestArrayType {
		[Theory]
		[InlineData("string[10]\r\n", "string", 10)]
		[InlineData("string[0]\r\n", "string", 0)]
		public void ArrayWithSize(string expected, string type, int size) {
			var result = new CodeStack().With(new ArrayTypeNode(new TypeNode(type), size)).Build();
			Assert.Equal(expected, result.ToString());
		}
	}
}
