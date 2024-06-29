using Albatross.CodeGen.TypeScript.Models;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class BooleanTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(Type type) => type == typeof(bool);
		public Expression Convert(Type type, TypeConverterFactory _, SyntaxTree syntaxTree) 
			=> syntaxTree.Type("bool");
	}
}
