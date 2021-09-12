using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Parameter {
		public Parameter() { }
		public Parameter(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public TypeScriptType Type { get; set; }
		public bool Optional { get; set; }
		public AccessModifier AccessModifier { get; set; }
	}
}
