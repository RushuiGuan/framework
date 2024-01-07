using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen {
	public abstract class ModuleCodeElement : IModuleCodeElement {
		public ModuleCodeElement(string name, string module) {
			this.Name = name;
			this.Module = module;
		}

		public string Name { get; set; }
		public string Module { get; set; }
		public string Tag { get; set; } = string.Empty;

		public virtual void Build() { }
		public abstract TextWriter Generate(TextWriter writer);
		public IEnumerator<IModuleCodeElement> GetEnumerator()
			=> Array.Empty<IModuleCodeElement>().AsEnumerable().GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	}
}
