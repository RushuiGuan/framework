using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestAssignmentBuilder {
		const string Assignment_Expected = @"a = 1
";
		[Fact]
		public void Assignment() {
			var result = new CodeStack().Begin(new AssignmentExpressionBuilder("a"))
				.With(new LiteralNode(1))
				.End().Build();
			Assert.Equal(Assignment_Expected, result.ToString());
		}

		const string AssignmentWithMemberAccess_Expected = @"this.a = 1
";
		[Fact]
		public void AssignmentWithMemberAccess() {
			var result = new CodeStack()
				.With(new ThisExpression()).With(new IdentifierNode("a"))
				.ToNewBegin(new AssignmentExpressionBuilder()).With(new LiteralNode(1)).End()
				.Build();
			Assert.Equal(AssignmentWithMemberAccess_Expected, result.ToString());
		}
	}
}