using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.Reflection;
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
}
