using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.CSharp.Model {
	public class Field { 
		public Field() { }
		public Field(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public DotNetType Type { get; set; }
		public AccessModifier Modifier { get; set; }
		public bool ReadOnly { get; set; } 
		public bool Static { get; set; }
		public bool Const { get; set; }
		public string Value { get; set; }
	}
}
