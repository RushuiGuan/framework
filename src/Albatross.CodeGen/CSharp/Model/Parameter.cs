using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.CSharp.Model {
	public class Parameter {
		public Parameter() { }
		public Parameter(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public DotNetType Type { get; set; }
		public ParameterModifier Modifier {get;set;}
	}
}
