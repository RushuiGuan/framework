using Albatross.Text;
using System.Collections.Generic;

namespace Albatross.CodeGen.Python.Models {
	public class Constructor : Method {
		public Constructor() : base("__init__") { }

		public List<Field> Fields { get; set; } = new List<Field>();
		public override ICodeElement BuildBody() {
			var block = new CodeBlock();
			foreach (var item in Fields) {
				block.Action += writer => writer.Code(item).WriteLine();
			}
			return block;
		}
	}
}