using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.Reflection;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class GenericTypeConverter : ITypeConverter {
		public int Precedence => 100;
		public bool Match(Type type) => type.IsGenericType;
		public ITypeExpression Convert(Type type, TypeConverterFactory factory) {
			return new GenericTypeExpression(type.GetGenericTypeDefinition().Name.GetGenericTypeName() + "_") {
				Arguments = new ListOfSyntaxNodes<ITypeExpression>(type.GetGenericArguments().Select(factory.Convert))
			};
		}
	}

	public class GenericTypeConverter2 : ITypeConverter2 {
		public int Precedence => 100;
		public bool Match(ITypeSymbol symbol) => symbol is INamedTypeSymbol named && named.IsGenericType;
		public ITypeExpression Convert(ITypeSymbol symbol, TypeConverterFactory2 factory) {
			return new GenericTypeExpression(symbol.Name + "_") {
				Arguments = new ListOfSyntaxNodes<ITypeExpression>(((symbol as INamedTypeSymbol)?.TypeArguments ?? []).Select(factory.Convert))
			};
		}
	}
}
