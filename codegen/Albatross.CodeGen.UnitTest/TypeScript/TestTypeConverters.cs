using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using Xunit;

namespace Albatross.CodeGen.UnitTest.TypeScript {
	public class TestTypeConverters {
		[Theory]
		[InlineData(@"class Example { public System.Collections.Generic.IEnumerable<string> P1 { get; } }")]
		[InlineData(@"class Example { public string[] P1 { get; } }")]
		[InlineData(@"class Example { public System.Collections.Generic.List<string> P1 { get; } }")]
		[InlineData(@"class Example { public System.Collections.Generic.ICollection<string> P1 { get; } }")]
		public void TestStringArray(string code) {
			var compilation = Albatross.CodeAnalysis.Extensions.CreateCompilation(code);

			var symbol = compilation.GetRequiredSymbol("Example");
			var p1Symbol = symbol.GetMembers().OfType<IPropertySymbol>().Where(x => x.Name == "P1").First();
			var factory = new TypeConverterFactory([new StringTypeConverter()], new Mock<ILogger<TypeConverterFactory>>().Object);
			var converter = new ArrayTypeConverter(compilation);
			Assert.True(converter.Match(p1Symbol.Type));
			var result = new ArrayTypeConverter(compilation).Convert(p1Symbol.Type, factory);
			Assert.IsType<ArrayTypeExpression>(result);
			var expression = result as ArrayTypeExpression;
			Assert.Equal(Defined.Types.String, expression?.Type);
		}

		[Theory]
		[InlineData(@"class Example { public byte[] P1 { get; } }")]
		public void TestByteArray(string code) {
			var compilation = Albatross.CodeAnalysis.Extensions.CreateCompilation(code);

			var symbol = compilation.GetRequiredSymbol("Example");
			var p1Symbol = symbol.GetMembers().OfType<IPropertySymbol>().Where(x => x.Name == "P1").First();
			var factory = new TypeConverterFactory([new StringTypeConverter(), new ArrayTypeConverter(compilation)], new Mock<ILogger<TypeConverterFactory>>().Object);
			var result = factory.Convert(p1Symbol.Type);
			Assert.Equal(Defined.Types.String, result);
		}

		[Theory]
		[InlineData(@"class Example { public string? P1 { get; } }")]
		[InlineData(@"class Example { public System.String? P1 { get; } }")]
		public void TestNullableString(string code) {
			var compilation = Albatross.CodeAnalysis.Extensions.CreateCompilation(code);

			var symbol = compilation.GetRequiredSymbol("Example");
			var p1Symbol = symbol.GetMembers().OfType<IPropertySymbol>().Where(x => x.Name == "P1").First();
			var factory = new TypeConverterFactory([new StringTypeConverter(), new ArrayTypeConverter(compilation)], new Mock<ILogger<TypeConverterFactory>>().Object);
			var result = factory.Convert(p1Symbol.Type);
			Assert.Equal(Defined.Types.String, result);
		}
	}
}
