using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.CSharp.Model {
	public class Constructor : Method {
		public Constructor(string name) : base(name) { }
		public Constructor() { }

		public Constructor BaseConstructor { get; set; }
	}
}
