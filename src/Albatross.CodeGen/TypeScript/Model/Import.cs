using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Import {
		public Import(TypeScriptFile file) {
			this.From = file;
		}
		public ISet<string> Items { get; set; } = new HashSet<string>();
		public TypeScriptFile From { get; set; }
	}
}
