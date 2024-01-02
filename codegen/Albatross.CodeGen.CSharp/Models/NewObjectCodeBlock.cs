using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Models {
	public class NewObjectCodeBlock<T> : ICodeElement {
		public NewObjectCodeBlock(string variableName) : this(variableName, string.Empty) { }
		public NewObjectCodeBlock(string variableName, string parameters) {
			VariableName = variableName;
			Parameters = parameters;
		}

		public string VariableName { get; set; }
		public string Parameters { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append("var ").Append(VariableName).Append(" = new ")
				.Append(typeof(T).FullName!)
				.OpenParenthesis().Append(Parameters).CloseParenthesis()
				.AppendLine(";");
			return writer;
		}
	}
}