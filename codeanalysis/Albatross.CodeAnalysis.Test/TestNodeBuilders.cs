using Albatross.CodeAnalysis.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
	public class TestNodeBuilders {
		[Theory]
		[InlineData("(\"a\", \"b\", \"c\", \"d\")", "a", "b", "c", "d")]
		[InlineData("()")]
		public void ArgumentList(string expected, params string[] arguments) {
			var result = new CodeStack().Begin(new ArgumentListBuilder()).With(arguments.Select(x => new LiteralNode(x)).ToArray()).End()
				.Build();
			Assert.Equal(expected, result.Trim());
		}


		[Theory]
		[InlineData("new object(\"a\")", "object", "a")]
		[InlineData("new object()", "object")]
		public void NewObject(string expected, string type, params string[] arguments) {
			var result = new CodeStack()
				.Begin(new NewObjectBuilder(type))
					.Begin(new ArgumentListBuilder())
						.With(arguments.Select(x => new LiteralNode(x)).ToArray())
					.End()
				.End().Build();
			Assert.Equal(expected, result.Trim());
		}

		[Theory]
		[InlineData("var test = 1", null, "test", 1)]
		[InlineData("int test = 1", "int", "test", 1)]
		public void VariableWithInitialization(string expected, string? type, string name, int value) {
			var node = new CodeStack().Begin(new VariableBuilder(type, name)).With(new LiteralNode(value)).End().Build();
			Assert.Equal(expected, node.ToString().Trim());
		}

		[Theory]
		[InlineData("var test", null, "test")]
		[InlineData("int test", "int", "test")]
		public void VariableDeclarationOnly(string expected, string? type, string name) {
			var node = new CodeStack().Begin(new VariableBuilder(type, name)).End().Build();
			Assert.Equal(expected, node.ToString().Trim());
		}

		[Theory]
		[InlineData("var test = new object()", "test", "object")]
		public void LocalVariableWithNewObject(string expected, string name, string type) {
			var node = new CodeStack()
				.Begin(new VariableBuilder(null, name)).Begin(new NewObjectBuilder(type)).End()
				.End().Build();
			Assert.Equal(expected, node.ToString().Trim());
		}

		[Theory]
		[InlineData("var test = new Test\r\n{\r\n\ta = 1\r\n}", "test", "Test")]
		public void LocalVariableWithNewObjectAndPropertyInitialization(string expected, string name, string type) {
			var node = new CodeStack()
				.Begin(new VariableBuilder(null, name))
					.Begin(new NewObjectBuilder(type))
						.Begin(new AssignmentExpressionBuilder("a")).With(new LiteralNode(1)).End()
					.End()
				.End().Build();
			Assert.Equal(expected, node.ToString().Trim());
		}

		


		[InlineData("this.a.b", true, "a", "b")]
		[InlineData("a.b", false, "a", "b")]
		[InlineData("this.a", true, "a")]
		[InlineData("a", false, "a")]
		[Theory]
		public void TestIdentifierCreation(string expected, bool memberAccess, params string[] names) {
			IdentifierNode? node = null;
			if (memberAccess) {
				node = new IdentifierNode();
			}
			foreach (var name in names) {
				if (node == null) {
					node = new IdentifierNode(name);
				} else {
					node = node.WithMember(name);
				}
			}
			Assert.Equal(expected, node?.ToString());
		}
	}
}
