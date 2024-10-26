using Albatross.Collections;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Import : ModuleCodeElement {
		public Import(string module) : this(Array.Empty<string>(), module) { }
		public Import(IEnumerable<string> names, string module) : base(string.Empty, module) {
			this.Names.AddRange(names);
		}
		public HashSet<string> Names { get; } = new HashSet<string>();

		public override TextWriter Generate(TextWriter writer) {
			if (Names.Count > 0) {
				writer.AppendLine();
				if (!string.IsNullOrEmpty(Module)) {
					writer.Append("from ").Append(Module).Space();
				}
				writer.Append("import ").WriteItems(Names, ", ");
			}
			return writer;
		}
	}
}