using Albatross.CodeAnalysis.Syntax;
using FluentAssertions;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
	public class TestStringInterpolation {
		[Fact]
		public void Simple() {
			var result = new StringInterpolationBuilder().Build([new IdentifierNode("test").Node, new LiteralNode("x").Node]).ToFullString();
			result.Should().Be("$\"{test}x\"");
		}


		[Fact]
		public void Multiple() {
			var result = new StringInterpolationBuilder().Build([
				new IdentifierNode("test").Node, 
				new LiteralNode("x").Node,
				new IdentifierNode("test").Node,
				new LiteralNode("x").Node
			]).ToFullString();
			result.Should().Be("$\"{test}x{test}x\"");
		}

		[Fact]
		public void WithFormat() {
			var result = new StringInterpolationBuilder().Build([
				new StringInterpolationNode("test", "yyyy-MM-dd").Node,
				new LiteralNode("x").Node,
				new StringInterpolationNode("test", "#,#0").Node,
				new LiteralNode("x").Node
			]).ToFullString();
			result.Should().Be("$\"{test:yyyy-MM-dd}x{test:#,#0}x\"");
		}
	}
}
