using Albatross.CodeGen.TypeScript.Models;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public interface ITypeConverter {
		public bool Match(Type type);
		public int Precedence { get; }
		public Expression Convert(Type type, TypeConverterFactory factory, SyntaxTree syntaxTree);
	}
}
