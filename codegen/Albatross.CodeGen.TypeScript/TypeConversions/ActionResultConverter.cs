using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using Albatross.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class ActionResultConverter : ITypeConverter {
		public int Precedence => 80;
		const string GenericDefinitionName = "Microsoft.AspNetCore.Mvc.ActionResult`1";
		public bool Match(ITypeSymbol symbol) => symbol.GetFullName() == "Microsoft.AspNetCore.Mvc.IActionResult"
			|| symbol.GetFullName() == "Microsoft.AspNetCore.Mvc.ActionResult"
			|| symbol.GetFullName() == "Microsoft.AspNetCore.Mvc.ActionResult<>";

		public ITypeExpression Convert(ITypeSymbol symbol, ITypeConverterFactory factory) {
			if (symbol.TryGetGenericTypeArguments(GenericDefinitionName, out var arguments)) {
				return factory.Convert(arguments[0]);
			} else {
				return Defined.Types.Any;
			}
		}
	}
}