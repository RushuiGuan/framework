using Albatross.CodeGen.TypeScript.Models;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public static class Extensions {
		public static TypeScriptType Convert(this TypeConverterFactory factory, Type type) {
			if (factory.TryGet(type, out var converter)) {
				return converter.Convert(type, factory);
			} else {
				return new TypeScriptType(type.Name);
			}
		}
	}
}
