using Albatross.CodeGen.Syntax;
using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class ListOfEnumItems : ListOfSyntaxNodes<EnumItemExpression> {
		protected override string Separator => ",\n";
		public ListOfEnumItems(IEnumerable<EnumItemExpression> items) : base(items) { }
	}
}