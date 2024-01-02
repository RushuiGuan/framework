using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class Parameter : ICodeElement {
		public Parameter(string name, PythonType? type) {
			this.Name = name;
			this.Type = type;
		}

		public string Name { get; set; }
		public PythonType? Type { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append(Name);
			if (Type != null) {
				writer.Append(": ").Code(Type);
			}
			return writer;
		}
	}
}