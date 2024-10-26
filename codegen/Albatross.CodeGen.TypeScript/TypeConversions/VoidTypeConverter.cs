using Albatross.CodeGen.Syntax;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class VoidTypeConverter : ITypeConverter {
		public int Precedence => 0;

		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			if (symbol.SpecialType == SpecialType.System_Void) {
				expression = Defined.Types.Void();
				return true;
			} else {
				expression = null;
				return false;
			}
		}
	}
}