using Albatross.CodeAnalysis.Syntax;
using FluentAssertions;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestUsingStatementNode {
		[Fact]
		public void UsingStatementWithVariableDeclaration() {
			var codestack = new CodeStack()
				.Begin(new UsingStatementBuilder())
					.Begin(new VariableBuilder("test"))
						.Begin(new NewObjectBuilder("object"))
						.End()
					.End()
				.End();
			codestack.Build().Should().Be(@"using (var test = new object())
{
}
");
		}

		[Fact]
		public void UsingStatementWithoutVariableDeclaration() {
			var codestack = new CodeStack()
				.Begin(new UsingStatementBuilder())
					.Complete(new NewObjectBuilder("object"))
				.End();
			codestack.Build().Should().Be(@"using (new object())
{
}
");
		}
	}
}