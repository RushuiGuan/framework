using Albatross.Text;
using System.Collections.Generic;
using System.IO;

namespace Albatross.CodeGen.TypeScript.Models {
	public class MethodDeclaration : ICodeElement {
		public MethodDeclaration(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public TypeExpression ReturnType { get; set; } = TypeExpression.Void();
		public AccessModifier AccessModifier { get; set; }
		public IEnumerable<ParameterDeclaration> Parameters { get; set; } = [];
        public bool Async { get; set; }
		public CodeBlock Body { get; set; } = new CodeBlock();

		public TextWriter Generate(TextWriter writer) {
			if (AccessModifier != AccessModifier.Public && AccessModifier != AccessModifier.None) {
				writer.Code(new AccessModifierSyntax(AccessModifier)).Space();
			}
			if (Async) { writer.Write("async "); }
			writer.Append(Name.CamelCase()).OpenParenthesis();
			writer.Code(new MethodParameterCollection(Parameters));
			writer.CloseParenthesis();
			if (!ReturnType.Equals(TypeExpression.Void())) {
				writer.Write(": ");
				writer.Code(ReturnType).Space();
			}
			using (var scope = writer.BeginScope()) {
				scope.Writer.Code(this.Body);
			}
			writer.WriteLine();
			return writer;
		}
	}
}
