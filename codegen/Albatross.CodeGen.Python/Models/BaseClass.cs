using System;
using System.IO;

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