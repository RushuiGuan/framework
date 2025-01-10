using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestArrayType {
		[Theory]
		[InlineData("string[0]", "string", 10)]
		public void ArrayWithSize(string expected, string type, int size) {
			var result = new CodeStack().With(new ArrayTypeNode(new TypeNode(type), size)).Build();
			Assert.Equal(expected, result.ToString());
		}
	}
}
