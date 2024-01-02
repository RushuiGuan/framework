using Albatross.Text;
using System;
using System.IO;
using System.Text.Json;

namespace Albatross.CodeGen.TypeScript.Models {
	public class LiteralValue : ICodeElement  {
		public LiteralValue(object value) {
			Value = value;
		}
		public object Value { get; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append(JsonSerializer.Serialize(this.Value));
			return writer;
		}
	}
}
