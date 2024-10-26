using Albatross.CodeGen.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class StringTypeConverter : SimpleTypeConverter {
		protected override IEnumerable<string> NamesToMatch => [
			"System.String",
			"System.Char",
			"System.Char[]",
			"System.Guid",
			"System.Byte[]"
		];

		protected override ITypeExpression GetResult(ITypeSymbol symbol) => Defined.Types.String(symbol.NullableAnnotation == NullableAnnotation.Annotated);
	}
}