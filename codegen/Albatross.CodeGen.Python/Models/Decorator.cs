using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Decorator : ICodeElement, IHasModule {
		public string Name { get; set; }
		public string Module { get; set; } = string.Empty;

		public Decorator(string name) {
			this.Name = name;
		}
		public TextWriter Generate(TextWriter writer) => writer.Append("@").Append(Name);
	}
}
