using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Declarations {
	public static class Extensions {
		public static IEnumerable<ImportExpression> Combine(this IEnumerable<ImportExpression> imports) {
			return imports.GroupBy(x=>x.Source).Select(items => new ImportExpression {
				Items = new ListOfSyntaxNodes<IdentifierNameExpression>(items.SelectMany(x => x.Items).Distinct()),
				Source = items.Key,
			});
		}
	}
}
