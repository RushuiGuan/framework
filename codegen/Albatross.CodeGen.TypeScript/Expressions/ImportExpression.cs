using Albatross.CodeGen.Syntax;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public record class ImportExpression : SyntaxNode, ICodeElement {
		public required ListOfSyntaxNodes<IdentifierNameExpression> Items { get; init; }
		public required ISourceExpression Source { get; init; }
		public override IEnumerable<ISyntaxNode> Children => [Items, Source];
		// import {format, parse} from 'date-fns';
		public override TextWriter Generate(TextWriter writer) {
			writer.Append("import ").Append("{ ").Code(Items).Append(" } ");
			writer.Append(" from ").Code(Source).Semicolon().WriteLine();
			return writer;
		}
	}
}
