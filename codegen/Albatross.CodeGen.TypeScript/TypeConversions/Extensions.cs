using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public static class Extensions {
		public static ITypeExpression Convert(this ITypeConverterFactory factory, ITypeSymbol symbol) {
			if (factory.TryGet(symbol, out var converter)) {
				return converter.Convert(symbol, factory);
			} else {
				throw new InvalidOperationException($"TypeConverter is not found for {symbol.GetFullName()}");
			}
		}
	}
}
