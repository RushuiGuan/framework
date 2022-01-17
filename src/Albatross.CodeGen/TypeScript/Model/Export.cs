using Albatross.CodeGen.Core;
using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Export : ICodeElement {
		public Export(string source, params string[] items) {
			this.Items = items;
			this.Source = source;
		}

		public string[] Items { get; set; }
		public string Source { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append("export ");
			if(Items.Length == 0) {
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
