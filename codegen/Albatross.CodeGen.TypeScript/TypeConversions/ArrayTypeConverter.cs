using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class ArrayTypeConverter : ITypeConverter {
		private readonly Compilation compilation;

		public ArrayTypeConverter(Compilation compilation) {
			this.compilation = compilation;
		}
		public int Precedence => 80;
		public bool Match(ITypeSymbol symbol) => symbol is IArrayTypeSymbol arrayTypeSymbol
			|| symbol.GetFullName() == "System.Collections.IEnumerable"
			|| symbol.IsDerivedFrom(compilation.GetTypeByMetadataName("System.Collections.IEnumerable"));

		public ITypeExpression Convert(ITypeSymbol symbol, ITypeConverterFactory factory) {
			ITypeExpression result;
			if (symbol is IArrayTypeSymbol arrayTypeSymbol) {
				result = factory.Convert(arrayTypeSymbol.ElementType);
			}else if(symbol.TryGetGenericTypeArguments("System.Collections.Generic.IEnumerable<>", out var arguments)) {
				result = factory.Convert(arguments[0]);
			} else if (symbol.TryGetGenericTypeArguments("System.Collections.Generic.List<>", out arguments)) {
				result = factory.Convert(arguments[0]);
			} else if (symbol.TryGetGenericTypeArguments("System.Collections.Generic.ICollection<>", out arguments)) {
				result = factory.Convert(arguments[0]);
			} else {
				result = Defined.Types.Any;
			}
			return new ArrayTypeExpression {
				Type = result,
			};
		}
	}
}
