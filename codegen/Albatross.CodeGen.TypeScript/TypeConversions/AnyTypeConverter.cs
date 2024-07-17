using Albatross.CodeGen.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class AnyTypeConverter : SimpleTypeConverter {
		protected override IEnumerable<string> NamesToMatch => [
			"System.Object",
			"System.Text.Json.JsonElement"
		];

		protected override ITypeExpression GetResult(ITypeSymbol symbol) => Defined.Types.Any(symbol.NullableAnnotation == NullableAnnotation.Annotated);
	}
}
