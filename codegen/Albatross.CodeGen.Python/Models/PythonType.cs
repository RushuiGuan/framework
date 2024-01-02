using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public record class PythonType : ICodeElement{
		public string Name { get; set; }
		public ICodeElement DefaultValue { get; set; } = new NoneValue();
		public PythonType(string name) {
			Name = name;
		}

		public TextWriter Generate(TextWriter writer) {
			writer.Append(Name);
			return writer;
		}
		
		public override string? ToString() {
			StringWriter writer = new StringWriter();
			return writer.Code(this).ToString();
		}
	}
}