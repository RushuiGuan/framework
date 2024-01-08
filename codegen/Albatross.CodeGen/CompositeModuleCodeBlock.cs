using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen {
	public class CompositeModuleCodeBlock : CompositeModuleCodeElement, IModuleCodeElement {
		public CompositeModuleCodeBlock() : base(string.Empty, string.Empty) { }

		public override TextWriter Generate(TextWriter writer) {
			foreach (var item in this) {
				writer.Code(item);
			}
			return writer;
		}
	}
}