using Albatross.CodeAnalysis.Syntax;
using FluentAssertions;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
	public class TestForEachBuilder {
		const string Simple_Expected = @"foreach (var item in items)
{
	int i = 0;
}
";
		[Fact]
		public void Simple() {
			var result = new CodeStack().Begin(new ForEachStatementBuilder("var", "item"))
					.With(new IdentifierNode("items"))
					.Begin(new VariableBuilder("int", "i")).With(new LiteralNode(0)).End()
				.End()
				.Build();
			result.Should().Be(Simple_Expected);
		}
	}
}
