using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Variable : CompositeModuleCodeElement {
		public Variable(string name) : base(name, string.Empty) {
			Type = My.Types.NoType();
		}

		public PythonType Type {
			get => Single<PythonType>(nameof(Type));
			set => Set(value, nameof(Type));
		}
		public override TextWriter Generate(TextWriter writer) => writer.Append(Name).Code(Type);
	}
}
