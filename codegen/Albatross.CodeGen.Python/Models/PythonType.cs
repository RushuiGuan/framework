using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class PythonType : CompositeModuleCodeElement {
		public PythonType(string name, string module) : base(name, module) {
			DefaultValue = new NoneValue();
		}
		public PythonType(string name) : this(name, string.Empty) {
		}

		public IModuleCodeElement DefaultValue {
			get => Single<IModuleCodeElement>(nameof(DefaultValue));
			set => Set(value, nameof(DefaultValue));
		}

		public override TextWriter Generate(TextWriter writer) {
			if (!string.IsNullOrEmpty(Name)) {
				writer.Append(":").Append(Name);
			}
			return writer;
		}
	}
}