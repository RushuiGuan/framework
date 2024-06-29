using Albatross.CodeGen.TypeScript.Models;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class VoidTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public TypeExpression Convert(Type type, TypeConverterFactory _, SyntaxTree syntaxTree) => TypeExpression.Void();
		public bool Match(Type type) => type == typeof(void);
	}
}
