using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
	public class TestVariableBuilder {
		[Theory]
		[InlineData("var test = 1", null, "test", 1)]
		[InlineData("int test = 1", "int", "test", 1)]
		public void VariableWithInitialization(string expected, string? type, string name, int value) {
			var node = new CodeStack().Begin(new VariableBuilder(type ?? string.Empty, name)).With(new LiteralNode(value)).End().Build();
			Assert.Equal(expected, node.ToString().Trim());
		}

		[Theory]
		[InlineData("var test", null, "test")]
		[InlineData("int test", "int", "test")]
		public void VariableDeclarationOnly(string expected, string? type, string name) {
			var node = new CodeStack().Begin(new VariableBuilder(type ?? string.Empty, name)).End().Build();
			Assert.Equal(expected, node.ToString().Trim());
		}

		[Fact]
		public void TestArrayVariable() {
			var cs = new CodeStack()
				.Complete(new VariableBuilder(new ArrayTypeNode("string"), "test"))
				.Begin(new AssignmentExpressionBuilder("test"))
					.With(new LiteralNode("a"))
				.End();
			Assert.Equal("string[] test\r\ntest = \"a\"", cs.Build().ToString().Trim());
		}
	}
}