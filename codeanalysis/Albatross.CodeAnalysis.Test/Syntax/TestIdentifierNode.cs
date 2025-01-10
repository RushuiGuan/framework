using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestIdentifierNode {
		[InlineData("this.a.b\r\n", true, "a", "b")]
		[InlineData("a.b\r\n", false, "a", "b")]
		[InlineData("this.a\r\n", true, "a")]
		[InlineData("a\r\n", false, "a")]
		[Theory]
		public void TestIdentifierCreation(string expected, bool memberAccess, params string[] names) {
			var cs = new CodeStack();
			if (memberAccess) {
				cs.With(new ThisExpression());
			}
			foreach (var name in names) {
				cs.With(new IdentifierNode(name));
			}
			cs.To(new MemberAccessBuilder());
			Assert.Equal(expected, cs.Build());
		}
	}
}
