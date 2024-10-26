using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class ParameterCollection : CompositeModuleCodeElement<Variable> {
		public ParameterCollection() : this(Array.Empty<Variable>()) { }
		public ParameterCollection(IEnumerable<Variable> parameters) : base(string.Empty, string.Empty, parameters) { }

		public override TextWriter Generate(TextWriter writer) {
			// writer.WriteItems<Variable>(this.Nodes, ", ", (w, item) => w.Code(item));
			return writer;
		}
	}
}