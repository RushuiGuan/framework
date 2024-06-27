using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Models {
	public class IdentifierName : ICodeElement {
		public IdentifierName(string name) {
			Name = name;
		}

		public string Name { get; }

		public TextWriter Generate(TextWriter writer) {
			writer.Append(Name);
			return writer;
		}
	}
}
