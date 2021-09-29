using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Method {
		public Method(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public TypeScriptType ReturnType { get; set; } = TypeScriptType.Void();
		public AccessModifier AccessModifier { get; set; }
		public IEnumerable<Parameter> Parameters { get; set; } = new Parameter[0];
        public bool Async { get; set; }
		public CodeBlock Body { get; set; } = new CodeBlock();
	}
}
