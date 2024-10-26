using Albatross.CodeAnalysis.Syntax;
using FluentAssertions;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestFieldBuilder {
		[Fact]
		public void FieldWithoutInitializer() {
			var codestack = new CodeStack()
				.Complete(new FieldDeclarationBuilder("int", "test"));
			codestack.Build().Should().Be("private int test;\r\n");
		}

		[Fact]
		public void FieldWithInitializer() {
			var codestack = new CodeStack()
				.Begin(new FieldDeclarationBuilder("int", "test"))
					.With(new LiteralNode(1))
				.End();
			codestack.Build().Should().Be("private int test = 1;\r\n");
		}
	}
}