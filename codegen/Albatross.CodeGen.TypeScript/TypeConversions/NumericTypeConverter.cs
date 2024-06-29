using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class NumericTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public TypeExpression Convert(Type type, TypeConverterFactory _, SyntaxTree syntaxTree) => TypeExpression.Number();
		public bool Match(Type type) => type.IsNumericType();
	}
}
