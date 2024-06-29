using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Models {
	public class VariableDeclaration : ICodeElement {

		public VariableDeclaration(string name, bool constant, TypeExpression? type) {
			Name = name;
			Constant = constant;
			Type = type;
		}

		public string Name { get; }
		public bool Constant { get; }
		public TypeExpression? Type { get; }
		public ICodeElement? Assignment { get; set; }

		public TextWriter Generate(TextWriter writer) {
			if (Constant) {
				writer.Append("const ");
			} else {
				writer.Append("var ");
			}
			writer.Append(Name);
			if(Type != null) {
				writer.Append(" : ").Code(Type);
			}
			if (Assignment != null) {
				writer.Append(" = ").Code(Assignment);
			}
			writer.AppendLine(";");
			return writer;
		}
	}
}
