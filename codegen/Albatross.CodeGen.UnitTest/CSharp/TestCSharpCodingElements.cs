using System.IO;
using Albatross.CodeGen.CSharp.Models;
using Xunit;

namespace Albatross.CodeGen.UnitTest.CSharp {
	public class TestCSharpCodingElements {

		[Theory]
		[InlineData(AccessModifier.Internal, "internal")]
		[InlineData(AccessModifier.Public, "public")]
		[InlineData(AccessModifier.Private, "private")]
		[InlineData(AccessModifier.Protected, "protected")]
		[InlineData(AccessModifier.Protected | AccessModifier.Internal, "protected internal")]
		[InlineData(AccessModifier.Private | AccessModifier.Internal, "private internal")]
		public void TestAccessModifier(AccessModifier accessModifier, string expected) {
			StringWriter writer = new StringWriter();
			writer.Code(new AccessModifierElement(accessModifier));
			string result = writer.ToString();
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("test;", "test;\n")]
		[InlineData("", "\n")]
		public void TestCodeBlock(string input, string expected) {
			StringWriter writer = new StringWriter();
			writer.Code(new CodeBlock(input));
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("a", "9", "var a = 9;\n")]
		public void TestAssignmentCodeBlock(string variable, string expression, string expected) {
			StringWriter writer = new StringWriter();
			writer.Code(new AssignmentCodeBlock(variable, expression));
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("item", "items", "foreach (var item in items) {\n}\n")]
		public void TestForEachCodeBlock(string itemVariable, string collectionVariable, string expected) {
			StringWriter writer = new StringWriter();
			writer.Code(new ForEachCodeBlock(itemVariable, collectionVariable));
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("a == b", "a = a - 1;", "b = b - 1;", "if (a == b) {\n\ta = a - 1;\n} else {\n\tb = b - 1;\n}\n")]
		[InlineData("a == b", "a = a - 1;", "", "if (a == b) {\n\ta = a - 1;\n}\n")]
		public void TestIfElseCodeBlock(string conditionExpression, string ifContent, string elseContent, string expected) {
			StringWriter writer = new StringWriter();
			writer.Code(new IfElseCodeBlock(conditionExpression, new CodeBlock(ifContent)) {
				ElseContent = string.IsNullOrEmpty(elseContent) ? null : new CodeBlock(elseContent),
			});
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("abc", "\"abc\"")]
		[InlineData("a\"bc", "\"a\\\"bc\"")]
		[InlineData("a\\bc", "\"a\\bc\"")]
		[InlineData("a\tbc", "\"a\\tbc\"")]
		[InlineData("a\nbc", "\"a\\nbc\"")]
		public void TestStringLiteral(string text, string expected) {
			StringWriter writer = new StringWriter();
			writer.Code(new StringLiteral(text));
			string actual = writer.ToString().RemoveCarriageReturn();
			Assert.Equal(expected, actual);
		}
	}
}
