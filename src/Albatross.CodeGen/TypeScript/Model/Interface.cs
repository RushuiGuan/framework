using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Interface {
		public Interface(string name) {
			this.Name = name;
		}
		public string Name { get; set; }
		public bool IsGeneric { get; set; }
		public IEnumerable<string> GenericTypes { get; set; } = new string[0];

		public IEnumerable<Property> Properties { get; set; } = new Property[0];
	}
}
