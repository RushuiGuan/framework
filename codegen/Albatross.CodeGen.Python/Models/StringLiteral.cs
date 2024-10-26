using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class StringLiteral : CompositeModuleCodeElement {
		public StringLiteral(string value) : base(string.Empty, string.Empty) {
			Value = value;
		}
		public string Value { get; set; }

		public void WriteContent(TextWriter writer) {
			foreach (char c in Value) {
				switch (c) {
					case '"':
						writer.Append("\\\"");
						break;
					case '\\':
						writer.Append("\\");
						break;
					case '\n':
						writer.Append("\\n");
						break;
					case '\t':
						writer.Append("\\t");
						break;
					default:
						writer.Append(c);
						break;
				}
			}
		}


		public override TextWriter Generate(TextWriter writer) {
			writer.AppendChar('"');
			WriteContent(writer);
			writer.AppendChar('"');
			return writer;
		}
	}
}