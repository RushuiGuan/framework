using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
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

		public static ITypeExpression Convert(this TypeConverterFactory2 factory, ITypeSymbol symbol) {
			if (factory.TryGet(symbol, out var converter)) {
				return converter.Convert(symbol, factory);
			} else {
				return new SimpleTypeExpression { Identifier = new IdentifierNameExpression(symbol.Name) };
			}
		}
	}
}
