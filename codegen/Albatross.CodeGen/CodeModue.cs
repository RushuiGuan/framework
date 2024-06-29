using System;

namespace Albatross.CodeGen {
	public class CodeModue : ICodeModule {
		public string Name { get; }

		public CodeModue(string name) {
			Name = name;
		}

		public IModuleCodeElement Add(ICodeElement element) {
			if (element is IModuleCodeElement) {
				throw new InvalidOperationException("Cannot add an module code element to another module");
			} else {
				return new ModuleCodeElement(this, element);
			}
		}
	}
}
