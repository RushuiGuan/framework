using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen {
	public class CodeLine : CompositeModuleCodeElement {
		public IModuleCodeElement Item {
			get => Single<IModuleCodeElement>(nameof(Item));
			set => Set(value, nameof(Item));
		}

		public CodeLine(IModuleCodeElement item) : base(string.Empty, string.Empty) {
			this.Item = item;
		}

		public override TextWriter Generate(TextWriter writer) {
			return writer.AppendLine().Code(Item);
		}
	}
}
