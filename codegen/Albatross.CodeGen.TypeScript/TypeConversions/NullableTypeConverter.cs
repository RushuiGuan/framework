using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class NullableTypeConverter : ITypeConverter {
		public int Precedence => 80;

		public bool Match(Type type) => type.IsGenericType
		&& type.GetGenericTypeDefinition() == typeof(Nullable<>);

		public Expression Convert(Type type, TypeConverterFactory factory, SyntaxTree syntaxTree) {
			if(type.GetNullableValueType(out var valueType)) {
				return new GenericTypeExpressionBuilder()
					.WithName(valueType.Name)
					.WithArgument(t => factory.Convert(syntaxTree, valueType))
					.Build(syntaxTree);
			} else {
				throw new ArgumentException("Nullable type converter should only be called with nullable types");
			}
		}
	}
}
