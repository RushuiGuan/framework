using Albatross.CodeGen.Core;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Parameter : ICodeElement{
		public Parameter(string name, TypeScriptType type) {
			this.Name = name;
			this.Type = type;
		}

		public string Name { get; set; }
		public TypeScriptType Type { get; set; }
		public bool Optional { get; set; }
		public AccessModifier AccessModifier { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Code(new AccessModifierElement(AccessModifier))
				.Append(Name).Append(" : ").Code(Type);

			return writer;
		}
	}
	public class MethodParameterCollection : ICodeElement {
		public MethodParameterCollection(IEnumerable<Parameter> parameters) {
			Parameters = parameters;
		}

		public IEnumerable<Parameter> Parameters { get; }

		public TextWriter Generate(TextWriter writer) {
			writer.WriteItems(Parameters, ", ", (w, item) => w.Code(item));
			return writer;
		}
	}
}
