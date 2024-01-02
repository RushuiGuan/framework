using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class Termination : ICodeElement {
		public TextWriter Generate(TextWriter writer) {
			writer.WriteLine(";");
			return writer;
		}
	}
}
