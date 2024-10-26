using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class ArrayTypeConverter : ITypeConverter {
		public int Precedence => 80;
		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			ITypeExpression typeExpression;
			if (symbol.TryGetCollectionElementType(out var elementType)) {
				typeExpression = factory.Convert(elementType!);
			} else if (symbol.IsCollection()) {
				typeExpression = Defined.Types.Any();
			} else {
				expression = null;
				return false;
			}
			expression = new ArrayTypeExpression {
				Type = typeExpression,
			};
			return true;
		}
	}
}