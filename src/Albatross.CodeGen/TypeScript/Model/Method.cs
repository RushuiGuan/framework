using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Method {
		public Method() { }
		public Method(string name) {
			Name = name;
		}

		public TypeScriptType ReturnType { get; set; } = TypeScriptType.Void();
		public string Name { get; set; }
		public AccessModifier AccessModifier { get; set; }
		public IEnumerable<Parameter> Parameters { get; set; }
        public bool Async { get; set; }
		public CodeBlock Body { get; set; } 
	}
}
