using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class NumericTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type.IsNumericType();
		public Expression Convert(Type type, TypeConverterFactory _, SyntaxTree syntaxTree)
			=> syntaxTree.Type("number");

	}
}
