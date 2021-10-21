using Albatross.CodeGen.Core;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Method : ICodeElement {
		public Method(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public TypeScriptType ReturnType { get; set; } = TypeScriptType.Void();
		public AccessModifier AccessModifier { get; set; }
		public IEnumerable<Parameter> Parameters { get; set; } = new Parameter[0];
        public bool Async { get; set; }
		public ICodeElement Body { get; set; } = new CodeBlock();

		public TextWriter Generate(TextWriter writer) {
			if (AccessModifier != AccessModifier.Public) {
				writer.Code(new AccessModifierElement(AccessModifier)).Space();
			}
			if (Async) { writer.Write("async "); }
			writer.Append(Name).OpenParenthesis();
			writer.Code(new MethodParameterCollection(Parameters));
			writer.CloseParenthesis();
			if (ReturnType == TypeScriptType.Void()) {
				writer.Write(":");
				writer.Code(ReturnType).Space();
			}
			writer.Code(Body);
			writer.WriteLine();
			return writer;
		}
	}
}
