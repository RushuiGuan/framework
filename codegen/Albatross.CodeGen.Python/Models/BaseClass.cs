using System;
using System.IO;

namespace Albatross.CodeGen.Python.Models {
	public class BaseClass : ICodeElement {
		public BaseClass(string name, string module) {
			this.Name = name;
			this.Module = module;
		}
		public string Name { get; set; }
		public string Module { get; set; }

		public void Build() {
			throw new NotImplementedException();
		}

		public TextWriter Generate(TextWriter writer) {
			throw new NotImplementedException();
		}
	}
}