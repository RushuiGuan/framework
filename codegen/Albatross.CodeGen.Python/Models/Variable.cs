using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Variable : CompositeModuleCodeElement {
		public Variable(string name) : base(name, string.Empty) {
			Type = My.Types.NoType();
		}
		public Variable(string name, PythonType type) : base(name, string.Empty) {
			this.Type = type;
		}
		public bool IsInstanceField { get; set; }

		public PythonType Type {
			get => Single<PythonType>(nameof(Type));
			set => Set(value, nameof(Type));
		}
		public override TextWriter Generate(TextWriter writer) {
			if (IsInstanceField) {
				writer.Append(My.Keywords.Self).Dot();
			}
			writer.Append(Name).Code(Type);
			return writer;
		}
	}
}