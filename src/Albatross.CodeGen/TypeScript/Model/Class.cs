using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Class {
		public Class(string name) {
			this.Name = name;
		}
		public string Name { get; set; }

		public AccessModifier AccessModifier { get; set; }
		public Class? BaseClass { get; set; }
		public bool IsGeneric { get; set; }

		public Constructor? Constructor { get; set; }
		public IEnumerable<string> Imports { get; set; } = new string[0];
		public IEnumerable<Property> Properties { get; set; } = new Property[0];
		public IEnumerable<Method> Methods { get; set; } = new Method[0];
	}
}
