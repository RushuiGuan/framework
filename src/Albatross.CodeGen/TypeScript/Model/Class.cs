using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Class {
		public bool Export { get; set; }
		public string Name { get; set; }

		public AccessModifier AccessModifier { get; set; }
		public Class BaseClass { get; set; }
		public string Namespace { get; set; }
		public bool IsGeneric { get; set; }

		public IEnumerable<string> Imports { get; set; }
		public Constructor Constructor { get; set; }
		public IEnumerable<Property> Properties { get; set; }
		public IEnumerable<Method> Methods { get; set; }
	}
}
