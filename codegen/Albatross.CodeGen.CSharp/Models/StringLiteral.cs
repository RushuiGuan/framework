using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Models {
	public class StringLiteral : ICodeElement {
		public StringLiteral(string value) {
			Value = value;
		}

		public string Value { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.AppendChar('"');
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
			writer.AppendChar('"');
			return writer;
		}
	}
}