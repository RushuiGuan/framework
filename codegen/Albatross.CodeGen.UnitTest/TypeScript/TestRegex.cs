using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Conversions;
using Albatross.CodeGen.TypeScript.Expressions;
using Xunit;

namespace Albatross.CodeGen.UnitTest.TypeScript {
	public class TestRegex {
		[Theory]
		[InlineData("a", true)]
		[InlineData("1", true)]
		[InlineData("@1/1", true)]
		[InlineData("@_/_", true)]
		[InlineData("@/a", false)]
		[InlineData("a1", true)]
		[InlineData("@angular/core", true)]
		[InlineData("@1angular/core", true)]
		[InlineData("@_angular/core", true)]
		[InlineData("@_angular", false)]
		[InlineData("@_angular/", false)]
		public void TestModuleName(string input, bool match) {
			var result = Albatross.CodeGen.TypeScript.Defined.Patterns.ModuleSource.IsMatch(input);
			Assert.Equal(match, result);
		}
	}
}
