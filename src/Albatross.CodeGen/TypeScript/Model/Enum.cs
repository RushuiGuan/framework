using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Enum {
		public Enum(string name) {
			this.Name = name;
		}
		public string Name { get; set; }
		public IEnumerable<string> Values { get; set; } = new string[0];
	}
}
