using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class NumericTypeConverter : ITypeConverter {
		public int Precedence => 0;
		public bool Match(ITypeSymbol symbol) => symbol.IsNumeric();

		public ITypeExpression Convert(ITypeSymbol type, ITypeConverterFactory _) => Defined.Types.Numeric;
	}
}
