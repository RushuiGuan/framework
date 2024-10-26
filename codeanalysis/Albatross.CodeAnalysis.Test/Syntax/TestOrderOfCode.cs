using Albatross.CodeAnalysis.Syntax;
using FluentAssertions;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestOrderOfCode {
		[Fact]
		public void Run() {
			var cs = new CodeStack();
			cs.Complete(new VariableBuilder("a"));
			cs.Complete(new VariableBuilder("b"));
			cs.Complete(new VariableBuilder("c"));
			var result = cs.Build();
			result.Should().Be(@"var a
var b
var c
");
		}
	}
}