using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestArgumentListBuilder {
		[Theory]
		[InlineData("(\"a\", \"b\", \"c\", \"d\")", "a", "b", "c", "d")]
		[InlineData("()")]
		public void ArgumentList(string expected, params string[] arguments) {
			var result = new CodeStack().Begin(new ArgumentListBuilder()).With(arguments.Select(x => new LiteralNode(x)).ToArray()).End()
				.Build();
			Assert.Equal(expected, result.Trim());
		}
	}
}
