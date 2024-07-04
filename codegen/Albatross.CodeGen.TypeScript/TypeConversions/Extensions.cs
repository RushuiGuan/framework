using Albatross.CodeGen.TypeScript.Expressions;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public static class Extensions {
		public static ITypeExpression Convert(this TypeConverterFactory factory, Type type) {
			if (factory.TryGet(type, out var converter)) {
				return converter.Convert(type, factory);
			} else {
				return new SimpleTypeExpression { Identifier = new IdentifierNameExpression(type.Name) };
			}
		}
	}
}
