using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class BooleanTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(ITypeSymbol symbol) => symbol.ToDisplayString() == "System.Boolean";
		public ITypeExpression Convert(ITypeSymbol symbol, ITypeConverterFactory _) => Defined.Types.Boolean;
	}
}
