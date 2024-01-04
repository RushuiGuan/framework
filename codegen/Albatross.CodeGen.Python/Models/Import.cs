using Albatross.Collections;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Import : ICodeElement {
		public string Module { get; set; }
		public HashSet<string> Names { get; set; } = new HashSet<string>();

		public Import(string module, IEnumerable<string> names) {
			this.Module = module;
			this.Names.AddRange(names);
		}

		public TextWriter Generate(TextWriter writer) {
			if(Names.Count > 0) {
				writer.Append("from ").Append(Module).Space()
					.Append("import ")
					.WriteItems(Names, ", ");
			}
			return writer;
		}
	}
}
