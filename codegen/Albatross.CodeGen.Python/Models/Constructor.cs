using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Constructor : Method {
		public Constructor() : this(string.Empty) { }
		public Constructor(string module) : base("__init__", module) { }

		public List<Field> InitFields { get; } = new List<Field>();

		public override void Build() {
			Static = false;
			this.InitFields.ForEach(x => {
				x.Static = false;
			});
			this.InitFields.ForEach(f => this.CodeBlock.Add(f));
			base.Build();
		}
		public override TextWriter Generate(TextWriter writer) {
			if (this.CodeBlock.Any()) {
				return base.Generate(writer);
			} else {
				return writer;
			}
		}
	}
}