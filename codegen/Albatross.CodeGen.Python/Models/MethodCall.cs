using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class MethodCall : ICodeElement, IHasModule {
		public MethodCall(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public string Module { get; set; } = string.Empty;

		public IEnumerable<ICodeElement> Parameters { get; set; } = new ICodeElement[0];
		public virtual TextWriter Generate(TextWriter writer) {
			writer.Append(Name).OpenParenthesis();
			writer.WriteItems(Parameters, ", ", (w, args) => w.Code(args));
			writer.CloseParenthesis();
			return writer;
		}
	}
}
