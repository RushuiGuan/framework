using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public static class Extensions {
		public static ITypeExpression Convert(this ITypeConverterFactory factory, ITypeSymbol symbol) {
			if (factory.TryGet(symbol, out var converter)) {
				return converter.Convert(symbol, factory);
			} else {
				return new SimpleTypeExpression { Identifier = new IdentifierNameExpression(symbol.Name) };
			}
		}
	}
}
