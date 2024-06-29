using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class ParameterDeclaration : ICodeElement{
		public ParameterDeclaration(string name, TypeExpression type, AccessModifier accessModifier) {
			this.Name = name;
			this.Type = type;
			this.AccessModifier = accessModifier;
		}

		public string Name { get; set; }
		public TypeExpression Type { get; set; }
		public bool Optional { get; set; }
		public AccessModifier AccessModifier { get; set; }

		public TextWriter Generate(TextWriter writer) {
			writer.Code(new AccessModifierSyntax(AccessModifier))
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
