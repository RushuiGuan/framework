using Albatross.CodeGen.TypeScript.Expressions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.CodeGen.UnitTest.TypeScript {
	public class TestFileNameSourceExpression {
		[Theory]
		[InlineData("test.ts", "'./test'")]
		[InlineData("./test.ts", "'./test'")]
		[InlineData(".\\test.ts", "'./test'")]
		[InlineData("lib\\test.ts", "'./lib/test'")]
		[InlineData("lib/test.ts", "'./lib/test'")]
		[InlineData("./lib/test.ts", "'./lib/test'")]
		public void Test(string name, string expected) {
			var expression = new FileNameSourceExpression(name);
			var result = new StringWriter().Code(expression).ToString();
			result.Should().Be(expected);
		}
	}
}