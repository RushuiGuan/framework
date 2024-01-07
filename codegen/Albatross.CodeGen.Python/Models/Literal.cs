using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class Literal : ModuleCodeElement {
		public Literal(object value) : base(string.Empty, string.Empty) {
			this.value = value;
		}
		object value { get; set; }

		public override TextWriter Generate(TextWriter writer) {
			writer.Write(value.ToString());
			return writer;
		}
	}
}
