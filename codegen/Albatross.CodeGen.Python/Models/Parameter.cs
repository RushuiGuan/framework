using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class Parameter : ICodeElement {
		public Parameter(string name) {
			this.Name = name;
		}

		public string Name { get; set; }
		public PythonType Type { get; set; } = My.Types.AnyType;
		public TextWriter Generate(TextWriter writer) => writer.Append(Name).Code(Type);
	}
}