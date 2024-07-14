using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class CollectionTypeConverter : ITypeConverter {
		private readonly Compilation compilation;

		public CollectionTypeConverter(Compilation compilation) {
			this.compilation = compilation;
		}
		public int Precedence => 80;
		public bool Match(ITypeSymbol symbol) => symbol is IArrayTypeSymbol arrayTypeSymbol && symbol.ToDisplayString() != "System.Byte[]"
			|| symbol.IsDerivedFrom(compilation.GetTypeByMetadataName("System.Collections.IEnumerable"));

		public ITypeExpression Convert(ITypeSymbol symbol, ITypeConverterFactory factory) {
			ITypeExpression result;
			if (symbol is IArrayTypeSymbol arrayTypeSymbol) {
				result = factory.Convert(arrayTypeSymbol.ElementType);
			}else if(symbol.TryGetGenericTypeArguments("System.Collections.Generic.IEnumerable<T>", out var arguments)) {
				result = factory.Convert(arguments[0]);
			} else if (symbol.TryGetGenericTypeArguments("System.Collections.Generic.List<T>", out arguments)) {
				result = factory.Convert(arguments[0]);
			} else if (symbol.TryGetGenericTypeArguments("System.Collections.Generic.ICollection<T>", out arguments)) {
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
