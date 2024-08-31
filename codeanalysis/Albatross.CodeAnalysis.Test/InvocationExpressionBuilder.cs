using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
	public class TestInvocationExpressionBuilder {
		const string MethodInvocation_Expected = @"test(1, 2, 3)
";
		[Fact]
		public void MethodInvocation() {
			var result = new CodeStack().Begin(new InvocationExpressionBuilder("test"))
					.Begin(new ArgumentListBuilder())
						.With(new LiteralNode(1), new LiteralNode(2), new LiteralNode(3))
					.End()
				.End().Build();
			Assert.Equal(MethodInvocation_Expected, result.ToString());
		}
		const string MethodInvocationWithMemberAccess_Expected = @"this.test(1, 2, 3)
";
		[Fact]
		public void MethodInvocationWithMemberAccess() {
			var result = new CodeStack().Begin(new InvocationExpressionBuilder(new IdentifierNode().WithMember("test")))
					.Begin(new ArgumentListBuilder())
						.With(new LiteralNode(1), new LiteralNode(2), new LiteralNode(3))
					.End()
				.End().Build();
			Assert.Equal(MethodInvocationWithMemberAccess_Expected, result.ToString());
		}
	}
}
