using Albatross.CodeAnalysis.Syntax;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.CodeAnalysis.Test {
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
			using (cs.Begin(new MethodDeclarationBuilder("string", "MyMethod")).NewScope()) {
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
		public void TestEmptyBegin() {
			var cs = new CodeStack();
			using (cs.Begin(new MethodDeclarationBuilder("string", "MyMethod")).NewScope()) {
				cs.Begin(new VariableBuilder("int", "test1")).With(new LiteralNode(1)).End();
				cs.Begin();
				cs.Begin(new VariableBuilder("int", "test2")).With(new LiteralNode(2)).End();
				cs.End();
				cs.Begin();
				cs.Begin(new VariableBuilder("int", "test3")).With(new LiteralNode(3)).End();
				cs.End();
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
			using (cs.Begin(new ClassDeclarationBuilder("MyClass")).NewScope()) {
				cs.With(new PropertyNode("string", "Name").Default());
				using (cs.Begin().NewScope()) {
					cs.Begin(new VariableBuilder("int", "test1")).With(new LiteralNode(1)).End();
					cs.Begin(new VariableBuilder("int", "test2")).With(new LiteralNode(2)).End();
					cs.Feed(new MethodDeclarationBuilder("string", "MyMethod"));
				}
			}
			Assert.Equal(TestFeed_Expected, cs.Build());
		}
	}
}
