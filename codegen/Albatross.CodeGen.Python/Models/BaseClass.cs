using Albatross.Collections;
using Albatross.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class BaseClass : IModuleCodeElement {
		public string Name { get; set; }
		public string Module { get; set; }
		public string Tag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void Build() {
			throw new NotImplementedException();
		}

		public TextWriter Generate(TextWriter writer) {
			throw new NotImplementedException();
		}
	}
}