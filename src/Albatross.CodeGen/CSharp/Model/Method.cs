using System.Collections.Generic;

namespace Albatross.CodeGen.CSharp.Model {
	public class Method {
		public Method(string name) {
			Name = name;
		}

		public DotNetType ReturnType { get; set; } = DotNetType.Void();
		public string Name { get; set; }
		public AccessModifier AccessModifier { get; set; }
		public IEnumerable<Parameter> Parameters { get; set; } = new Parameter[0];
        public bool Async { get; set; }
		public bool Static { get; set; }
		public bool Virtual { get; set; }
		public bool Override { get; set; }
		public CSharpCodeBlock CodeBlock { get; set; } = new CSharpCodeBlock();
	}
}
