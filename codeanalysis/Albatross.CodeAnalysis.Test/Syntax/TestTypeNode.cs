using Albatross.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.CodeAnalysis.Test.Syntax {
	public class TestTypeNode {
		[Theory]
		[InlineData("var", "var")]
		[InlineData("int", "int")]
		[InlineData("string", "string")]
		public void TestNormal(string type, string expected) {
			var cs = new CodeStack().With(new TypeNode(type)).Build();
			Assert.Equal(expected, cs.ToString().Trim());
		}

		[Theory]
		[InlineData("var", "var")]
		[InlineData("string", "string?")]
		public void TestNullableReferenceType(string type, string expected) {
			var cs = new CodeStack().With(new TypeNode(type).NullableReferenceType()).Build();
			Assert.Equal(expected, cs.ToString().Trim());
		}
	}
}
