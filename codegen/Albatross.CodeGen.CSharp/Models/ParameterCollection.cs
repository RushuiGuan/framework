using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.CSharp.Models {
	public class ParameterCollection : ICodeElement {
		public ParameterCollection(IEnumerable<Parameter> parameters) {
			Parameters = parameters;
		}

		public IEnumerable<Parameter> Parameters { get; }

		public TextWriter Generate(TextWriter writer) {
			writer.WriteItems(Parameters, ", ", (w, item) => w.Code(item));
			return writer;
		}
	}
}