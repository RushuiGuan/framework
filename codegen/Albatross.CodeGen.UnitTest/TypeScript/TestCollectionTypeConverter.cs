using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Albatross.Hosting.Test;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using Xunit;

namespace Albatross.CodeGen.UnitTest.TypeScript {
	public class TestCollectionTypeConverter {
		/*
		 * public string[] P2 { get; set; }
	public List<string> P3 { get; set; }
	public ICollection<string> P4 { get; set; }
}");
		 */

		[Theory]
		[InlineData(@"class Example { public System.Collections.Generic.IEnumerable<string> P1 { get; } }")]
		[InlineData(@"class Example { public string[] P1 { get; } }")]
		[InlineData(@"class Example { public System.Collections.Generic.List<string> P1 { get; } }")]
		[InlineData(@"class Example { public System.Collections.Generic.ICollection<string> P1 { get; } }")]
		public void Run(string code) {
			var compilation = Albatross.CodeAnalysis.Extensions.CreateCompilation(code);

			var symbol = compilation.GetRequiredSymbol("Example");
			var p1Symbol = symbol.GetMembers().OfType<IPropertySymbol>().Where(x => x.Name == "P1").First();
			var factory = new TypeConverterFactory([new StringTypeConverter()]);
			var converter = new CollectionTypeConverter(compilation);
			Assert.True(converter.Match(p1Symbol.Type));
			var result = new CollectionTypeConverter(compilation).Convert(p1Symbol.Type, factory);
			Assert.IsType<ArrayTypeExpression>(result);
			var expression = result as ArrayTypeExpression;
			Assert.Equal(Defined.Types.String, expression?.Type);
		}
	}
}
