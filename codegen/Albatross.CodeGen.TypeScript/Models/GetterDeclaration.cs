using Albatross.Text;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class GetterDeclaration : ICodeElement {
		public GetterDeclaration(string name, AccessModifier accessModifier, TypeExpression type) {
			Name = name;
			AccessModifier = new AccessModifierSyntax(accessModifier);
			Type = type;
		}
		public CodeBlock Body { get; set; } = new CodeBlock();

		public string Name { get; set; }
		public AccessModifierSyntax AccessModifier { get; set; }
		public TypeExpression Type { get; }

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
