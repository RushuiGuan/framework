using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class NumericTypeConverter : SimpleTypeConverter {
		protected override IEnumerable<string> NamesToMatch => [];
		protected override ITypeExpression GetResult(ITypeSymbol _) => Defined.Types.Numeric();
		protected override bool IsMatch(ITypeSymbol symbol) => symbol.IsNumeric();
	}
}