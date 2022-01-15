using Albatross.CodeGen.Core;
using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Model {
	public class Getter : ICodeElement {
		public Getter(string name, AccessModifier accessModifier, TypeScriptType type) {
			Name = name;
			AccessModifier = new AccessModifierElement(accessModifier);
			Type = type;
		}
		public CodeBlock Body { get; init; } = new CodeBlock();

		public string Name { get; set; }
		public AccessModifierElement AccessModifier { get; set; }
		public TypeScriptType Type { get; }

		public TextWriter Generate(TextWriter writer) {
			writer.Code(AccessModifier).Append("get ").Append(Name).Append("():").Code(Type);
			using (var scope = writer.BeginScope()) {
				scope.Writer.Code(Body);
			}
			writer.WriteLine();
			return writer;
		}
	}
}
