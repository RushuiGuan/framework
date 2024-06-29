using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Models {
	public class ImportExpression : ICodeElement {
		internal ImportExpression() { }

		public required ISet<IdentifierNameExpression> Items { get; init; }
		public required IdentifierNameExpression Source { get; init; }

		// import {format, parse} from 'date-fns';
		public TextWriter Generate(TextWriter writer) {
			writer.Append("import ");
			writer.Append("{ ").WriteItems(Items, ", ").Append(" } ");
			writer.Append(" from ").Code(Source).Semicolon().WriteLine();
			return writer;
		}
	}
}
