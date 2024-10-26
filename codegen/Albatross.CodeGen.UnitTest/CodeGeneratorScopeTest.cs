using System;
using System.IO;
using Xunit;

namespace Albatross.CodeGen.UnitTest {
	public class CodeGeneratorScopeTest {
		const string SingleScopeResult = "{\n\tint i = 100;\n}\n";

		[Fact]
		public void SingleScope() {
			StringWriter writer = new StringWriter();
			CodeGeneratorScope scope = new CodeGeneratorScope(writer, args => args.WriteLine("{"), args => args.WriteLine("}"));
			using (scope) {
				scope.Writer.WriteLine("int i = 100;");
			}
			string result = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(SingleScopeResult, result);
		}


		const string NestedScopeResult = "{\n\tint i = 100;\n\ttest {\n\t\tint a = 200;\n\t}\n}\n";
		[Fact]
		public void NestedScope() {
			StringWriter writer = new StringWriter();
			using (CodeGeneratorScope scope = new CodeGeneratorScope(writer, args => args.WriteLine("{"), args => args.WriteLine("}"))) {

				scope.Writer.WriteLine("int i = 100;");
				using (var child = scope.Writer.BeginScope("test")) {
					child.Writer.WriteLine("int a = 200;");
				}
			}
			string result = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(NestedScopeResult, result);
		}
	}
}