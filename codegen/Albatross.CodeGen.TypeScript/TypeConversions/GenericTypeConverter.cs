using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class GenericTypeConverter : ITypeConverter {
		public int Precedence => 100;
		public bool Match(Type type) => type.IsGenericType;
		public Expression Convert(Type type, TypeConverterFactory factory, SyntaxTree syntaxTree) {
			var name = type.GetGenericTypeDefinition().Name.GetGenericTypeName() + "_";
			var builder = new GenericTypeExpressionBuilder().WithName(name);
			foreach (var argument in type.GetGenericArguments()) {
				builder.WithArgument(t => factory.Convert(syntaxTree, argument));
			}
			return builder.Build(syntaxTree);
		}
	}
}
