using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Models {
	public class ExportExpression : ICodeElement {
		internal ExportExpression() { }

		public required IEnumerable<IdentifierNameExpression> Items { get; init; }
		public required IdentifierNameExpression Source { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append("export ");
			if(!Items.Any()) {
				writer.Append("*");
			} else {
				writer.Append("{");
				writer.WriteItems(Items, ", ", (w, item) => w.Append(item));
				writer.Append("}");
			}
			writer.Append(" from ").Literal(Source).AppendLine(";");
			return writer;
		}
	}
}
