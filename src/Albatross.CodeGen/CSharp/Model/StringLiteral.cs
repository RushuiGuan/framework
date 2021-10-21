using Albatross.CodeGen.Core;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Model {
	public class StringLiteral : ICodeElement {
		public StringLiteral(string value) {
			Value = value;
		}

		public string Value { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.AppendChar('"');
			foreach(char c in Value) {
				if(c == '"') {
					writer.Append("\\\"");
				} else {
					writer.Append(c);
				}
			}
			writer.AppendChar('"');
			return writer;
		}
	}
}
