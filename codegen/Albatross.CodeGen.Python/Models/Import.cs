using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Import : ModuleCodeElement {
		public Import(IEnumerable<string> names, string module) : base(string.Empty, module){
			this.Names.AddRange(names);
		}
		public List<string> Names { get; } = new List<string>();

		public override TextWriter Generate(TextWriter writer) {
			if(Names.Count > 0) {
				writer.Append("from ").Append(Module).Space()
					.Append("import ")
					.WriteItems(Names, ", ");
			}
			return writer;
		}
	}
}
