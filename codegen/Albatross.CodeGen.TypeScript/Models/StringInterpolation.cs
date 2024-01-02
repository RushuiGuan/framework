using Albatross.Text;
using System;
using System.IO;
using System.Text.Json;

namespace Albatross.CodeGen.TypeScript.Models {
	public class StringInterpolation : ICodeElement  {
		public StringInterpolation(string value) {
			Value = value;
		}
		public string Value { get; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append("`").Append(Value).Append("`");
			return writer;
		}
	}
}
