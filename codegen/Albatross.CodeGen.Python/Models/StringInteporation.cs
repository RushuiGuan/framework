using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class StringInteporation : CompositeModuleCodeElement {
		public StringInteporation(params IModuleCodeElement[] elems) : base(string.Empty, string.Empty) {
			AddRange(elems);
		}

		public override TextWriter Generate(TextWriter writer) {
			writer.Append("f\"");
			foreach (var item in this) {
				if (item is StringLiteral literal) {
					literal.WriteContent(writer);
				} else {
					writer.Append("{").Code(item).Append("}");
				}
			}
			writer.Append('"');
			return writer;
		}
	}
}