using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Microsoft.CodeAnalysis;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class ActionResultConverter : ITypeConverter {
		public int Precedence => 80;

		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			var name = symbol.GetFullName();
			if (name == "Microsoft.AspNetCore.Mvc.IActionResult" || name == "Microsoft.AspNetCore.Mvc.ActionResult") {
				expression = Defined.Types.Any();
				return true;
			} else if (symbol.TryGetGenericTypeArguments("Microsoft.AspNetCore.Mvc.ActionResult<>", out var arguments)) {
				expression = factory.Convert(arguments[0]);
				return true;
			} else {
				expression = null;
				return false;
			}
		}
	}
}