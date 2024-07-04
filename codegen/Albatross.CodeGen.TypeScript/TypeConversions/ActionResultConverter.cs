using System;
using Microsoft.AspNetCore.Mvc;
using Albatross.CodeGen.TypeScript.Expressions;

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
}