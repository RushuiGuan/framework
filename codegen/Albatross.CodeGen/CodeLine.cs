using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen {
	public class CodeLine : CompositeModuleCodeElement {
		private readonly IModuleCodeElement item;

		public CodeLine(IModuleCodeElement item) : base(string.Empty, string.Empty) {
			this.item = item;
		}

		public override TextWriter Generate(TextWriter writer) {
			return writer.AppendLine().Code(item);
		}
	}
}
