using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class NullableTypeConverter : ITypeConverter {
		public int Precedence => 80;

		public TypeExpression Convert(Type type, TypeConverterFactory factory, SyntaxTree syntaxTree) {
			if(type.GetNullableValueType(out var valueType)) {
				var result = factory.Convert(valueType);
				result.IsNullable = true;
				return result;
			} else {
				throw new ArgumentException("Nullable type converter should only be called with nullable types");
			}
		}
		public bool Match(Type type) => type.IsGenericType 
			&& type.GetGenericTypeDefinition() == typeof(Nullable<>);
	}
}
