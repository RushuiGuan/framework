using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestNewArrayBuilder {
		[Theory]
		[InlineData("new string[]\r\n{\r\n\t\"a\",\r\n\t\"b\"\r\n}")]
		public void NewStringArray(string expected) {
			var node = new CodeStack()
				.Begin(new NewArrayBuilder("string"))
					.With(new LiteralNode("a"), new LiteralNode("b"))
				.End()
			.Build();
			Assert.Equal(expected, node.ToString().Trim());
		}
		[Theory]
		[InlineData("new string[0]")]
		public void EmptyStringArray(string expected) {
			var node = new CodeStack()
				.Begin(new NewArrayBuilder("string")).End()
			.Build();
			Assert.Equal(expected, node.ToString().Trim());
		}

		[Theory]
		[InlineData("new int[]\r\n{\r\n\t1,\r\n\t2\r\n}")]
		public void NewIntArray(string expected) {
			var node = new CodeStack()
				.Begin(new NewArrayBuilder("int"))
					.With(new LiteralNode(1), new LiteralNode(2))
				.End()
			.Build();
			Assert.Equal(expected, node.ToString().Trim());
		}
	}
}
