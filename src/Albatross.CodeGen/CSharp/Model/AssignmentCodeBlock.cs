using Albatross.CodeGen.Core;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Model {
	public class AssignmentCodeBlock : ICodeBlock {
		public AssignmentCodeBlock(string variableName, string expression) {
			VariableName = variableName;
			Expression = expression;
		}

		public string VariableName { get; set; }
		public string Expression { get; set; }
	}

	public class WriteAssignmentCodeBlock : WriteCodeBlock<AssignmentCodeBlock> {
		public override void Run(TextWriter writer, AssignmentCodeBlock source) {
			writer.Append("var ").Append(source.VariableName).Append(" = ")
				.Append(source.Expression)
				.AppendLine(";");
		}
	}
}
