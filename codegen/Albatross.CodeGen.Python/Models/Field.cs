using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class Field : Assignment {
		public Field(string name, PythonType type, IModuleCodeElement value)
			: base(new Variable(name, type) { IsInstanceField = true }, value) {
		}

		public bool Static { get; set; }
		public override void Build() {
			base.Build();
			this.Variable.IsInstanceField = !this.Static;
		}
		public override TextWriter Generate(TextWriter writer) {
			writer.AppendLine();
			return base.Generate(writer);
		}
	}
}