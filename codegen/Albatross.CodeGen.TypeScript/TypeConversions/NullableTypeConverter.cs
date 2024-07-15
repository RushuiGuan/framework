using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class NullableTypeConverter : ITypeConverter {
		public int Precedence => 80;
		public const string NullableDefinitionName = "System.Nullable<>";
		public bool Match(ITypeSymbol symbol) => symbol is INamedTypeSymbol named
			&& named.IsGenericType
			&& named.OriginalDefinition.GetFullName() == NullableDefinitionName;

		public ITypeExpression Convert(ITypeSymbol type, ITypeConverterFactory factory) {
			if (type.TryGetGenericTypeArguments(NullableDefinitionName, out var arguments)) {
				return factory.Convert(arguments[0]);
			} else {
				throw new Exception("Nullable type must have a generic argument");
			}
		}
	}
}
