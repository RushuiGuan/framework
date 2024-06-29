using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using System;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class GenericTypeConverter : ITypeConverter {
		public int Precedence => 100;
		public bool Match(Type type) => type.IsGenericType;
		public TypeExpression Convert(Type type, TypeConverterFactory factory, SyntaxTree syntaxTree) {
			var result = new TypeExpression(type.GetGenericTypeDefinition().Name.GetGenericTypeName() + "_") {
				IsGeneric = true,
				GenericTypeArguments = type.GetGenericArguments().Select(x => factory.Convert(x)).ToArray(),
			};
			return result;
		}
	}
}
