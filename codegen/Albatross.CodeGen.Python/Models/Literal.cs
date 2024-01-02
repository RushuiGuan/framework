using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class Literal : ICodeElement{
		object value { get; set; }

		public Literal(object value) {
			this.value = value;
		}

		public TextWriter Generate(TextWriter writer) {
			writer.Write(value.ToString());
			return writer;
		}
	}
}
