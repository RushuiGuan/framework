using System;
using Microsoft.AspNetCore.Mvc;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using Albatross.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class ActionResultConverter : ITypeConverter {
		public int Precedence => 80;
		public bool Match(Type type) => type == typeof(IActionResult)
			|| type == typeof(ActionResult)
			|| type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ActionResult<>);

		public ITypeExpression Convert(Type type, TypeConverterFactory factory) {
			if (type == typeof(ActionResult) || type == typeof(IActionResult)) {
				return Defined.Types.Any;
			} else {
				return factory.Convert(type.GetGenericArguments()[0]);
			}
		}
	}

	public class ActionResultConverter2 : ITypeConverter2 {
		public int Precedence => 80;
		const string GenericDefinitionName = "Microsoft.AspNetCore.Mvc.ActionResult`1";
		public bool Match(ITypeSymbol symbol) => symbol.ToDisplayString() == "Microsoft.AspNetCore.Mvc.IActionResult"
			|| symbol.ToDisplayString() == "Microsoft.AspNetCore.Mvc.ActionResult"
			|| symbol.TryGetGenericTypeArguments(GenericDefinitionName, out var _);

		public ITypeExpression Convert(ITypeSymbol symbol, TypeConverterFactory2 factory) {
			if (symbol.TryGetGenericTypeArguments(GenericDefinitionName, out var arguments)) {
				return factory.Convert(arguments[0]);
			} else {
				return Defined.Types.Any;
			}
		}
	}
}