using Albatross.CodeAnalysis.Syntax;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestCodeStack {
		const string TestScope_Expected = @"public string MyMethod()
{
	int test1 = 1;
	int test2 = 2;
}
";
		[Fact]
		public void TestScope() {
			var cs = new CodeStack();
			using (cs.NewScope(new MethodDeclarationBuilder("string", "MyMethod").Public())) {
				cs.Begin(new VariableBuilder("int", "test1")).With(new LiteralNode(1)).End();
				cs.Begin(new VariableBuilder("int", "test2")).With(new LiteralNode(2)).End();
			}
			Assert.Equal(TestScope_Expected, cs.Build());
		}

		const string TestEmptyBegin_Expected = @"public string MyMethod()
{
	int test1 = 1;
	int test2 = 2;
	int test3 = 3;
}
";
		[Fact]
		public void TestNoOpNodeBuilder() {
			var cs = new CodeStack();
			using (cs.NewScope(new MethodDeclarationBuilder("string", "MyMethod").Public())) {
				using (cs.NewScope()) {
					cs.Begin(new VariableBuilder("int", "test1")).With(new LiteralNode(1)).End();
					cs.Begin(new VariableBuilder("int", "test2")).With(new LiteralNode(2)).End();
					cs.Begin(new VariableBuilder("int", "test3")).With(new LiteralNode(3)).End();
				}
			}
			Assert.Equal(TestEmptyBegin_Expected, cs.Build());
		}
		const string TestFeed_Expected = @"public class MyClass
{
	public string Name { get; set; }

	public string MyMethod()
	{
		int test1 = 1;
		int test2 = 2;
	}
}
";
		[Fact]
		public void TestFeed() {
			var cs = new CodeStack();
			using (cs.NewScope(new ClassDeclarationBuilder("MyClass").Public())) {
				cs.With(new PropertyNode("string", "Name").Default());
				using (cs.NewScope()) {
					cs.Begin(new VariableBuilder("int", "test1")).With(new LiteralNode(1)).End();
					cs.Begin(new VariableBuilder("int", "test2")).With(new LiteralNode(2)).End();
					cs.To(new MethodDeclarationBuilder("string", "MyMethod").Public());
				}
			}
			Assert.Equal(TestFeed_Expected, cs.Build());
		}
	}
}