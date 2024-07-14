using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public interface ITypeConverter {
		public bool Match(ITypeSymbol namedTypeSymbol);
		public int Precedence { get; }
		public ITypeExpression Convert(ITypeSymbol type, TypeConverterFactory factory);
	}
}
