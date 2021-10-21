using Albatross.CodeGen.Core;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Model {
	public class AssignmentCodeBlock : ICodeElement {
		public AssignmentCodeBlock(string variableName, string expression) {
			VariableName = variableName;
			Expression = expression;
		}

		public string VariableName { get; set; }
		public string Expression { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append("var ").Append(VariableName).Append(" = ")
				.Append(Expression)
				.AppendLine(";");
			return writer;
		}
	}
}
