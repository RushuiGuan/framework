using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class StringInterpolationExpression : ICodeElement  {
		public StringInterpolationExpression(string value) {
			Value = value;
		}
		public string Value { get; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append("`").Append(Value).Append("`");
			return writer;
		}
	}
}
