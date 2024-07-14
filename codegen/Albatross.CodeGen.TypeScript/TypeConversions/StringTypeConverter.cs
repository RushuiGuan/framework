using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class StringTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(ITypeSymbol symbol) => symbol.ToDisplayString() == "System.String";
		public ITypeExpression Convert(ITypeSymbol symbol, TypeConverterFactory _) => Defined.Types.String;
	}
}
