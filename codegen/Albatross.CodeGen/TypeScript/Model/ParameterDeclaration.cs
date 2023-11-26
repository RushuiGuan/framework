using Albatross.CodeGen.Core;
using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Model {
	public class ParameterDeclaration : ICodeElement{
		public ParameterDeclaration(string name, TypeScriptType type, AccessModifier accessModifier) {
			this.Name = name;
			this.Type = type;
			this.AccessModifier = accessModifier;
		}

		public string Name { get; set; }
		public TypeScriptType Type { get; set; }
		public bool Optional { get; set; }
		public AccessModifier AccessModifier { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Code(new AccessModifierElement(AccessModifier))
				.Append(Name).Append(": ").Code(Type);

			return writer;
		}
	}

	public class MethodParameterCollection : ICodeElement {
		public MethodParameterCollection(IEnumerable<ParameterDeclaration> parameters) {
			Parameters = parameters;
		}

		public IEnumerable<ParameterDeclaration> Parameters { get; }

		public TextWriter Generate(TextWriter writer) {
			writer.WriteItems(Parameters, ", ", (w, item) => w.Code(item));
			return writer;
		}
	}
}
