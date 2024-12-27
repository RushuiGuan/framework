using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestInterfaceDeclaration {
		const string InterfaceDeclaration_Expected = @"public interface ITest
{
	void test();
}
";
		[Fact]
		public void MethodInvocation() {
			var result = new CodeStack().Begin(new InterfaceDeclarationBuilder("ITest").Public())
				.Begin(new MethodDeclarationBuilder("void", "test").UsedByInterface()).End()
				.End().Build();
			Assert.Equal(InterfaceDeclaration_Expected, result.ToString());
		}
	}
}
