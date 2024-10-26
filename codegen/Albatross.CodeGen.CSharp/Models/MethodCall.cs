using Albatross.Text;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.CodeGen.CSharp.Models {
	public class MethodCall : ICodeElement {
		public MethodCall(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public bool Await { get; set; }
		public IEnumerable<ICodeElement> Parameters { get; set; } = new ICodeElement[0];

		public virtual TextWriter Generate(TextWriter writer) {
			if (Await) {
				writer.Write("await ");
			}
			writer.Append(Name).OpenParenthesis();
			writer.WriteItems(Parameters, ", ", action: (w, args) => w.Code(args));
			writer.CloseParenthesis();
			return writer;
		}
	}
}