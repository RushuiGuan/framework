using Albatross.CodeGen.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class BooleanTypeConverter : SimpleTypeConverter {
		protected override IEnumerable<string> NamesToMatch => ["System.Boolean"];
		protected override ITypeExpression GetResult(ITypeSymbol symbol) => Defined.Types.Boolean();
	}
}