using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class ParameterCollection : ICodeElement{
		public ParameterCollection(IEnumerable<Parameter> parameters) {
			Parameters = new List<Parameter>(parameters);
		}

		public List<Parameter> Parameters { get; }

		public TextWriter Generate(TextWriter writer) {
			writer.WriteItems(Parameters, ", ", (w, item) => w.Code(item));
			return writer;
		}

		public void InsertSelfWhenMissing() {
			if (!Parameters.Any() || Parameters.First().Name != "self") {
				Parameters.Insert(0, new Parameter("self", null));
			}
		}
	}
}
