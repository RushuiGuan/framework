using Albatross.CodeGen.Core;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Termination : ICodeElement {
		public TextWriter Generate(TextWriter writer) {
			writer.WriteLine(";");
			return writer;
		}
	}
}
