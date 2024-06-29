using Albatross.CodeGen.TypeScript.Models;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class ActionResultConverter : ITypeConverter {
		public int Precedence => 80;
		public bool Match(Type type) => type == typeof(IActionResult) 
			|| type == typeof(ActionResult) 
			|| type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ActionResult<>);

		public Expression Convert(Type type, TypeConverterFactory factory, SyntaxTree syntaxTree) {
			if (type == typeof(ActionResult) || type == typeof(IActionResult)) {
				return syntaxTree.AnyType();
			} else {
				return factory.Convert(syntaxTree, type.GetGenericArguments()[0]);
			}
		}
	}
}