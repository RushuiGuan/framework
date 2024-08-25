using Microsoft.CodeAnalysis;
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
		[InlineData("new object ()", "object")]
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
		[InlineData("var test = 1", "test", 1)]
		public void LocalVariable(string expected, string name, int value) {
			var node = new CodeStack().Begin(new LocalVariableBuilder(name)).With(new LiteralNode(value)).End().Build();
			Assert.Equal(expected, node.ToString().Trim());
		}

		[Theory]
		[InlineData("var test = new object ()", "test", "object")]
		public void LocalVariableWithNewObject(string expected, string name, string type) {
			var node = new CodeStack()
				.Begin(new LocalVariableBuilder(name)).Begin(new NewObjectBuilder(type)).End()
				.End().Build();
			Assert.Equal(expected, node.ToString().Trim());
		}

		[Theory]
		[InlineData("var test = new Test\r\n{\r\n\ta = 1\r\n}", "test", "Test")]
		public void LocalVariableWithNewObjectAndPropertyInitialization(string expected, string name, string type) {
			var node = new CodeStack()
				.Begin(new LocalVariableBuilder(name))
					.Begin(new NewObjectBuilder(type))
						.Begin(new AssignmentExpressionBuilder("a")).With(new LiteralNode(1)).End()
					.End()
				.End().Build();
			Assert.Equal(expected, node.ToString().Trim());
		}
	}
}
