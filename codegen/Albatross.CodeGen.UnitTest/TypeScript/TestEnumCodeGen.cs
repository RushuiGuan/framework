using Albatross.CodeAnalysis;
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
			var compilation = Albatross.CodeAnalysis.Extensions.CreateCompilation(code);
			var symbol = compilation.GetRequiredSymbol("MyEnum1");
			var result = new ConvertEnumToTypeScriptEnum().Convert(symbol);

			Assert.Equal("MyEnum1", result.Identifier.Name);
			Assert.Collection(result.Items,
				v => {
					Assert.Equal("New", v.Identifier.Name);
					Assert.True(v.Expression is StringLiteralExpression literal && literal.Value == "New");
				},
				v => {
					Assert.Equal("Brand", v.Identifier.Name);
					Assert.True(v.Expression is StringLiteralExpression literal && literal.Value == "Brand");
				},
				v => {
					Assert.Equal("Best", v.Identifier.Name);
					Assert.True(v.Expression is StringLiteralExpression literal && literal.Value == "Best");
				});
		}

		[Fact]
		public void TestIntEnumValue() {
			string code = @"
	public enum MyEnum1 {
		New = 1, Brand = 2, Best = 3
	}"; var compilation = Albatross.CodeAnalysis.Extensions.CreateCompilation(code);
			var symbol = compilation.GetRequiredSymbol("MyEnum1");
			var result = new ConvertEnumToTypeScriptEnum().Convert(symbol);

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
