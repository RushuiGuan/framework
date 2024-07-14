using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using Albatross.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class ActionResultConverter : ITypeConverter {
		public int Precedence => 80;
		const string GenericDefinitionName = "Microsoft.AspNetCore.Mvc.ActionResult`1";
		public bool Match(ITypeSymbol symbol) => symbol.ToDisplayString() == "Microsoft.AspNetCore.Mvc.IActionResult"
			|| symbol.ToDisplayString() == "Microsoft.AspNetCore.Mvc.ActionResult"
			|| symbol.TryGetGenericTypeArguments(GenericDefinitionName, out var _);

		public ITypeExpression Convert(ITypeSymbol symbol, TypeConverterFactory factory) {
			if (symbol.TryGetGenericTypeArguments(GenericDefinitionName, out var arguments)) {
				return factory.Convert(arguments[0]);
			} else {
				return Defined.Types.Any;
			}
		}
	}
}