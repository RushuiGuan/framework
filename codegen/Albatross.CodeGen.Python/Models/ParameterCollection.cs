using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class ParameterCollection : List<Variable>, IModuleCodeElement {
		public ParameterCollection() { }
		public ParameterCollection(IEnumerable<Variable> parameters) : base(parameters) { }

		public string Name => string.Empty;
		public string Module { get; set; } = string.Empty;
		public string Tag { get; set; } = string.Empty;

		public void Build() { }

		public TextWriter Generate(TextWriter writer) {
			writer.WriteItems<Variable>(this, ", ", (w, item) => w.Code(item));
			return writer;
		}

		IEnumerator<IModuleCodeElement> IEnumerable<IModuleCodeElement>.GetEnumerator()
			=> this.Cast<IModuleCodeElement>().GetEnumerator();
	}
}
