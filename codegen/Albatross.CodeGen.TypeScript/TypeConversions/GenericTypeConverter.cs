using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class GenericTypeConverter : ITypeConverter {
		public int Precedence => 100;

		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			if (symbol is INamedTypeSymbol named && named.IsGenericType) {
				expression = new GenericTypeExpression(symbol.Name) {
					Arguments = new ListOfSyntaxNodes<ITypeExpression>(((symbol as INamedTypeSymbol)?.TypeArguments ?? []).Select(factory.Convert))
				};
				return true;
			} else {
				expression = null;
				return false;
			}
		}
	}
}