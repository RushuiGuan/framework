using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.CSharp.Models;
using Albatross.CodeGen.TypeScript.Conversions;
using Albatross.CodeGen.TypeScript.Expressions;
using Xunit;

namespace Albatross.CodeGen.UnitTest.TypeScript {
	public enum MyEnum {
		New, Brand, Best
	}
	public class TestEnumCodeGen {
		[Fact]
		public void TestStringEnumValue() {
			string code = @"
	using System.Text.Json.Serialization;
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum MyEnum1 {
		New, Brand, Best
	}";
			var compilation = CodeAnalysis.Symbols.Extensions.CreateCompilation(code);
			var symbol = compilation.GetRequiredSymbol("MyEnum1");
			var result = new ConvertEnum().Convert(symbol);

			Assert.Equal("MyEnum1", result.Identifier.Name);
			Assert.Collection(result.Items,
				v => {
					Assert.Equal("New", v.Identifier.Name);
					Assert.IsType<StringLiteralExpression>(v.Expression);
					Assert.Equal("New", ((StringLiteralExpression)v.Expression).Value);
				},
				v => {
					Assert.Equal("Brand", v.Identifier.Name);
					Assert.IsType<StringLiteralExpression>(v.Expression);
					Assert.Equal("Brand", ((StringLiteralExpression)v.Expression).Value);
				},
				v => {
					Assert.Equal("Best", v.Identifier.Name);
					Assert.IsType<StringLiteralExpression>(v.Expression);
					Assert.Equal("Best", ((StringLiteralExpression)v.Expression).Value);
				});
		}

		[Fact]
		public void TestIntEnumValue() {
			string code = @"
	public enum MyEnum1 {
		New = 1, Brand = 2, Best = 3
	}"; var compilation = CodeAnalysis.Symbols.Extensions.CreateCompilation(code);
			var symbol = compilation.GetRequiredSymbol("MyEnum1");
			var result = new ConvertEnum().Convert(symbol);

			Assert.Equal("MyEnum1", result.Identifier.Name);
			Assert.Collection(result.Items,
				v => {
					Assert.Equal("New", v.Identifier.Name);
					Assert.True(v.Expression is NumberLiteralExpression literal && literal.Value == 1);
				},
				v => {
					Assert.Equal("Brand", v.Identifier.Name);
					Assert.True(v.Expression is NumberLiteralExpression literal && literal.Value == 2);
				},
				v => {
					Assert.Equal("Best", v.Identifier.Name);
					Assert.True(v.Expression is NumberLiteralExpression literal && literal.Value == 3);
				});
		}
	}
}
