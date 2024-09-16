using Albatross.CodeAnalysis.Syntax;
using FluentAssertions;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestOrderOfCode {
		[Fact]
		public void Run() {
			var cs = new CodeStack();
			cs.Complete(new VariableBuilder(null, "a"));
			cs.Complete(new VariableBuilder(null, "b"));
			cs.Complete(new VariableBuilder(null, "c"));
			var result = cs.Build();
			result.Should().Be(@"var a
var b
var c
");
		}
	}
}
