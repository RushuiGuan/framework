using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.Reflection;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class NullableTypeConverter : ITypeConverter {
		public int Precedence => 80;

		public bool Match(Type type) => type.IsGenericType
		&& type.GetGenericTypeDefinition() == typeof(Nullable<>);

		public ITypeExpression Convert(Type type, TypeConverterFactory factory) {
			if(type.GetNullableValueType(out var valueType)) {
				return new SimpleTypeExpression {
					Identifier = new IdentifierNameExpression(valueType.Name),
					Optional = true,
				};
			} else {
				throw new ArgumentException("Nullable type converter should only be called with nullable types");
			}
		}
	}
}
