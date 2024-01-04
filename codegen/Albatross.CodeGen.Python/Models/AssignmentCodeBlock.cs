using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class AssignmentCodeBlock : ICodeElement {
		public AssignmentCodeBlock(string variableName, ICodeElement expression) {
			fieldName = variableName;
			Expression = expression;
		}

		public string fieldName { get; set; }
		public ICodeElement Expression { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append(fieldName).Append(" = ").Code(Expression);
			return writer;
		}
	}
}
