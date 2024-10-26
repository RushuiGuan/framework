using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class NullableTypeConverter : ITypeConverter {
		public int Precedence => 80;
		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			ITypeExpression? typeExpression = null;
			if (symbol.TryGetNullableValueType(out var valueType)) {
				typeExpression = factory.Convert(valueType!);
			} else if (symbol.IsNullableReferenceType()) {
				typeExpression = factory.Convert(symbol.WithNullableAnnotation(NullableAnnotation.None));
			}
			if (typeExpression != null) {
				expression = new MultiTypeExpression(typeExpression, Defined.Types.Undefined());
				return true;
			} else {
				expression = null;
				return false;
			}
		}
	}
}