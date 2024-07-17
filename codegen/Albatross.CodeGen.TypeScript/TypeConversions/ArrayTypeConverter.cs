using Albatross.CodeGen.Syntax;
using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class ArrayTypeConverter : ITypeConverter {
		const string IEnumerableName = "System.Collections.IEnumerable";
		private readonly Compilation compilation;

		public ArrayTypeConverter(Compilation compilation) {
			this.compilation = compilation;
		}
		public int Precedence => 80;
		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			ITypeExpression elementType;
			if (symbol is IArrayTypeSymbol arrayTypeSymbol) {
				elementType = factory.Convert(arrayTypeSymbol.ElementType);
			} else if (symbol.TryGetGenericTypeArguments("System.Collections.Generic.IEnumerable<>", out var arguments)) {
				elementType = factory.Convert(arguments[0]);
			} else if (symbol.TryGetGenericTypeArguments("System.Collections.Generic.List<>", out arguments)) {
				elementType = factory.Convert(arguments[0]);
			} else if (symbol.TryGetGenericTypeArguments("System.Collections.Generic.ICollection<>", out arguments)) {
				elementType = factory.Convert(arguments[0]);
			} else {
				var name = symbol.GetFullName();
				if(name == IEnumerableName || symbol.IsDerivedFrom(compilation.GetTypeByMetadataName(IEnumerableName))) {
					elementType = Defined.Types.Any();
				} else {
					expression = null;
					return false;
				}
			}
			expression =  new ArrayTypeExpression {
				Type = elementType,
			};
			return true;
		}
	}
}
