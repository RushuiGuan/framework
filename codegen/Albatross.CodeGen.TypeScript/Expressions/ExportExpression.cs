using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class ExportExpression : SyntaxNode, IExpression {
		public ListOfSyntaxNodes<IdentifierNameExpression> Items { get; init; } = new();
		public required FileNameSourceExpression Source { get; set; }
		public override IEnumerable<ISyntaxNode> Children => [Items, Source];

		public override TextWriter Generate(TextWriter writer) {
			writer.Append("export ");
			if (Items.HasAny) {
				writer.Append("{").Code(Items).Append("}");
			} else {
				writer.Append("*");
			}
			writer.Append(" from ").Code(Source).AppendLine(";");
			return writer;
		}
	}
}