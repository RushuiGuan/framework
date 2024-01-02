using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Variable : ICodeElement {
		public string Name { get; set; }
		public PythonType? Type { get; set; }
		public Variable(string name) {
			this.Name = name;
		}
		public TextWriter Generate(TextWriter writer) {
			writer.Append(Name);
			if (Type != null) {
				writer.Append(": ").Code(Type);
			}
			return writer;
		}
	}
}
