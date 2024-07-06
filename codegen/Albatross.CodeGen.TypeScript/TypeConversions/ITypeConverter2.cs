using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public interface ITypeConverter2 {
		public bool Match(ITypeSymbol namedTypeSymbol);
		public int Precedence { get; }
		public ITypeExpression Convert(ITypeSymbol type, TypeConverterFactory2 factory);
	}
}
