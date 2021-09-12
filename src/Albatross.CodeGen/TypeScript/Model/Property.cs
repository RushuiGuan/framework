using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Property {
		public bool IsPrivate { get; set; }
		public string Name { get; set; }
		public TypeScriptType Type { get; set; }
		public bool Optional { get; set; }
	}
}
