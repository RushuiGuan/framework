using Albatross.CodeGen.TypeScript.Expressions;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public interface ITypeConverter {
		public bool Match(Type type);
		public int Precedence { get; }
		public ITypeExpression Convert(Type type, TypeConverterFactory factory);
	}
}
