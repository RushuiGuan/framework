using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class AsyncTypeConverter : ITypeConverter {
		public const string GenericDefinitionName = "System.Threading.Tasks.Task<>";
		public int Precedence => 90;

		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			var name = symbol.GetFullName();
			if (name == "System.Threading.Tasks.Task") {
				expression = Defined.Types.Void();
				return true;
			} else if (symbol.TryGetGenericTypeArguments(GenericDefinitionName, out var arguments)) {
				expression = factory.Convert(arguments[0]);
				return true;
			} else {
				expression = null;
				return false;
			}
		}
	}
}