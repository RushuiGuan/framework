using Albatross.CodeGen.Syntax;
using Albatross.CodeAnalysis;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class NullableTypeConverter : ITypeConverter {
		public int Precedence => 80;
		public const string NullableDefinitionName = "System.Nullable<>";
		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			if (symbol.TryGetGenericTypeArguments(NullableDefinitionName, out var arguments)) {
				expression = factory.Convert(arguments[0]);
				return true;
			} else {
				expression = null;
				return false;
			}
		}
	}
}
