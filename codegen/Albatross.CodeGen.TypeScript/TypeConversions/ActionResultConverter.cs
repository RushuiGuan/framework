using Albatross.CodeGen.TypeScript.Models;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class ActionResultConverter : ITypeConverter {
		public int Precedence => 80;
		public bool Match(Type type) => type == typeof(ActionResult) || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ActionResult<>);
		public TypeScriptType Convert(Type type, TypeConverterFactory factory) {
			if (type == typeof(ActionResult)) {
				return TypeScriptType.Any();
			} else {
				Type innerType = type.GetGenericArguments()[0];
				return factory.Convert(innerType);
			}
		}
	}
}
